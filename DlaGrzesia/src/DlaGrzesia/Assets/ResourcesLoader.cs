using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DlaGrzesia.Assets
{
    public class ResourcesLoader
    {
        private readonly ContentManager contentManager;

        public ResourcesLoader(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public T Load<T>() where T : class, new()
        {
            var properties = typeof(T)
                .GetProperties()
                .Select(x => (
                    Attribute: x.GetCustomAttributes(typeof(ResourceAttribute), false).SingleOrDefault() as ResourceAttribute,
                    Property: x))
                .Where(x => x.Attribute != null)
                .ToList();

            var resources = new T();
            
            foreach (var (attribute, property) in properties)
            {
                object content;

                if (property.PropertyType == typeof(Tileset))
                {
                    content = LoadTileset(property.Name, attribute);
                }
                else
                {
                    content = LoadContent(property.PropertyType, property.Name);
                }

                property.GetSetMethod(true).Invoke(resources, new object[] { content });
            }

            return resources;
        }

        private object LoadTileset(string resourceName, ResourceAttribute attribute)
        {
            var textures = new List<Texture2D>();
            Point tileSize;

            if (attribute is TilesetAttribute tilesetAttribute)
            {
                if (tilesetAttribute.IsMultipart)
                {
                    for (var i = 0; i < tilesetAttribute.FilesCount; i++)
                    {
                        var texturePart = contentManager.Load<Texture2D>($"{resourceName}.{i}");
                        textures.Add(texturePart);
                    }
                }
                else
                {
                    var texture = contentManager.Load<Texture2D>(resourceName);
                    textures.Add(texture);
                }

                tileSize = tilesetAttribute.TileSize;
            }
            else
            {
                var texture = contentManager.Load<Texture2D>(resourceName);
                textures.Add(texture);
                tileSize = texture.Bounds.Size;
            }

            return new Tileset(textures, tileSize);
        }

        private object LoadContent(Type contentType, string contentName)
        {
            return typeof(ContentManager)
                .GetMethod(nameof(ContentManager.Load))
                .MakeGenericMethod(contentType)
                .Invoke(contentManager, new object[] { contentName });
        }
    }
}
