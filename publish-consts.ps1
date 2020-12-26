$root = $PSScriptRoot
$publishDir = "$root\publish"
$compressionAssembly = "System.IO.Compression.FileSystem"

function PublishGame
{
    Param
    (
        [ValidateNotNullOrEmpty()][String] $outputDir,
        [Switch] $selfContained
    )

    Add-Type -AssemblyName $compressionAssembly

    $outputPath = "$publishDir\$outputDir"
    $archivePath = "$outputPath.zip"

    Remove-Item -Path $outputPath -Recurse -ErrorAction SilentlyContinue
    Remove-Item -Path $archivePath -ErrorAction SilentlyContinue

    if ($selfContained.IsPresent)
    {
        dotnet publish "$root\DlaGrzesia" --nologo -o $outputPath --configuration Release -p:PublishSingleFile=true --runtime win-x64
    }
    else
    {
        dotnet publish "$root\DlaGrzesia" --nologo -o $outputPath --configuration Release --no-self-contained -p:PublishSingleFile=true --runtime win-x64
    }

    Remove-Item -Path "$outputPath\*.pdb"
    [IO.Compression.ZipFile]::CreateFromDirectory($outputPath, $archivePath)
}