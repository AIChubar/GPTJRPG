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

    #$null = python3 -m pip install --upgrade pip > $null
    # Install openai module without displaying any output
    #$null = python3 -m pip install --upgrade openai > $null
    # Install jsons module without displaying any output
    #$null = python3 -m pip install --upgrade jsons > $null
    
    python3 api_request_narrative.py
    python3 api_request_main_character.py
    python3 api_request_levels.py
    python3 api_request_unit_data.py
    python3 api_request_quests.py
    python3 api_request_dialogue.py
 } else {
    Write-Host "StreamingAssets folder not found in any subdirectory."
}