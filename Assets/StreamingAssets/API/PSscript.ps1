# Define a function to find the StreamingAssets folder
function Find-StreamingAssetsFolder {
    param (
        [string]$FolderPath
    )

    # Get all directories in the current folder
    $Directories = Get-ChildItem -Path $FolderPath -Directory -Recurse

    # Loop through each directory to find the StreamingAssets folder
    foreach ($Directory in $Directories) {
        $StreamingAssetsPath = Join-Path -Path $Directory.FullName -ChildPath "StreamingAssets\API"
        if (Test-Path $StreamingAssetsPath) {
            return $StreamingAssetsPath
        }
    }

    return $null
}

# Find the StreamingAssets folder
$StreamingAssetsPath = Find-StreamingAssetsFolder -FolderPath .

# Check if StreamingAssets folder was found
if ($StreamingAssetsPath) {
    Set-Location $StreamingAssetsPath
    python3 main.py
} else {
    Write-Host "StreamingAssets folder not found in any subdirectory."
}