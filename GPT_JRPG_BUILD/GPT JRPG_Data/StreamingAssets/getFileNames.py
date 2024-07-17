import os
import json

def create_json_structure(folder_path):
    if not os.path.isdir(folder_path):
        print("Error: The provided path is not a directory.")
        return

    structure = {}
    for root, dirs, files in os.walk(folder_path):
        current_dir = os.path.relpath(root, folder_path)
        current_structure = []

        for file in files:
            if not file.endswith(".meta"):  # Skip files with ".meta" in their name
                file_name, file_ext = os.path.splitext(file)
                if file_ext == ".png":
                    file = file_name  # Remove the ".png" extension
                current_structure.append(file)

        structure[current_dir] = current_structure

    return structure

def save_json_structure(structure, output_file):
    with open(output_file, 'w') as json_file:
        json.dump(structure, json_file, indent=4)

if __name__ == "__main__":
    folder_path = "D:\\Unity_proj\\GPT JRPG\\Assets\\Sprite\\Monsters corrected"
    output_file = "D:\\Unity_proj\\GPT JRPG\\Assets\\StreamingAssets\\units_list.json"

    structure = create_json_structure(folder_path)
    save_json_structure(structure, output_file)