import json
import os
import pandas as pd

worlds_folder_path_4o_0_6 = os.path.join(os.pardir, "Worlds_4o_0_6")
worlds_folder_path_4o_1_2 = os.path.join(os.pardir, "Worlds_4o_1_2")
worlds_folder_path_3_5_1_2 = os.path.join(os.pardir, "Worlds_3_5_1_2")
worlds_folder_path_3_5_0_6 = os.path.join(os.pardir, "Worlds_3_5_0_6")
with open(os.path.join(os.pardir, "API", "units.json")) as f:
    units_atlas = json.load(f)

with open(os.path.join(os.pardir, "API", "units_stats.json")) as f:
    units_stats = json.load(f)


def is_valid_unit(unit, _units):
    first_attr = unit['firstAttribute']
    second_attr = unit['secondAttribute']
    third_attr = unit['thirdAttribute']
    unit_class = unit['unitClass']
    unit_power = unit['powerLevel']

    validity = [0, 0, 0, 0, 0]

    if unit_power in units_stats["powerLevelAttributes"]:
        validity[0] = 1
    else:
        validity[0] = 0
    if unit_class in units_stats["unitClass"]:
        validity[1] = 1

    if first_attr not in _units:
        return validity
    else:
        validity[2] = 1
    if second_attr not in _units[first_attr]:
        return validity
    else:
        validity[3] = 1
    if third_attr and third_attr not in _units[first_attr][second_attr]:
        return validity
    else:
        validity[4] = 1


    return validity


def is_valid_world(_narrative, _protagonist, units, data_frame):
    data_frame = pd.concat(
        [data_frame, pd.DataFrame([is_valid_unit(_narrative['antagonist'], units_atlas)], columns=['a', 'b', 'c', 'd', 'e'])])

    for unit in _narrative['antagonistGroup']['units']:
        data_frame = pd.concat([data_frame, pd.DataFrame([is_valid_unit(unit, units_atlas)], columns=['a', 'b', 'c', 'd', 'e'])])

    for level in units['levelsUnits']:
        for group in level['enemyGroups']:
            if 'units' not in group:
                continue
            for unit in group['units']:
                data_frame = pd.concat([data_frame, pd.DataFrame([is_valid_unit(unit, units_atlas)], columns=['a', 'b', 'c', 'd', 'e'])])

    for unit in _protagonist['protagonistGroup']['units']:
        if is_valid_unit(unit, units_atlas):
            data_frame = pd.concat([data_frame, pd.DataFrame([is_valid_unit(unit, units_atlas)], columns=['a', 'b', 'c', 'd', 'e'])])

    return data_frame

def is_valid_world_set(worlds_path):
    df = pd.DataFrame([], columns=['a', 'b', 'c', 'd', 'e'])

    for name in os.listdir(worlds_path):
        if ".meta" in name:
            continue
        json_file_path = os.path.join(worlds_path, name, 'UnitData.json')
        if os.path.isdir(os.path.join(worlds_path, name)) and os.path.exists(json_file_path):
            with open(json_file_path, 'r') as json_file:
                units = json.load(json_file)

        json_file_path = os.path.join(worlds_path, name, 'Narrative.json')
        if os.path.isdir(os.path.join(worlds_path, name)) and os.path.exists(json_file_path):
            with open(json_file_path, 'r') as json_file:
                narrative = json.load(json_file)

        json_file_path = os.path.join(worlds_path, name, 'MainCharacter.json')
        if os.path.isdir(os.path.join(worlds_path, name)) and os.path.exists(json_file_path):
            with open(json_file_path, 'r') as json_file:
                protagonist = json.load(json_file)

        df = is_valid_world(narrative, protagonist, units, df)

    tmp = df[(df.a == 1) & (df.b == 1) & (df.c == 1) & (df.d == 1) & (df.e == 1)]
    result = worlds_path + "\n"
    result += f'The DataFrame has {df.shape[0]} rows.' + "\n"
    result += f"A(power): {len(df) - df.a.sum()}, {round((len(df) - df.a.sum()) / (len(df) - len(tmp)), 3) * 100} %" + "\n"
    result += f"B(class): {len(df) - df.b.sum()}, {round((len(df) - df.b.sum()) / (len(df) - len(tmp)), 3) * 100} %" + "\n"
    c = len(df) - df.c.sum()
    result += f"C(first): {c}, {round(c / (len(df) - len(tmp)), 3) * 100} %" + "\n"
    d = len(df) - df.d.sum() - c
    result += f"D(second): {d}, {round(d / (len(df) - len(tmp)), 3) * 100} %" + "\n"
    e = len(df) - df.e.sum() - d
    result += f"E(third): {e}, {round(e / (len(df) - len(tmp)), 3) * 100} %" + "\n"

    result += f" valid {len(tmp)}, {round((len(tmp)) / len(df), 3) * 100} %" + "\n"
    result += f"class+atr {len(df[(df.b == 0) & (df.e == 0)])}" + "\n"
    result += f"class+power {len(df[(df.b == 0) & (df.a == 0)])}" + "\n"
    result += f"power+atr {len(df[(df.a == 0) & (df.e == 0)])}" + "\n"
    result += f"power+class+atr {len(df[(df.a == 0) & (df.e == 0) & (df.b == 0)])}" + "\n"

    return result + "\n"


with open("correctness_result", 'w') as file:
    pass

with open("correctness_result", 'a') as file:
    file.write(is_valid_world_set(worlds_folder_path_4o_0_6))
    file.write(is_valid_world_set(worlds_folder_path_4o_1_2))
    file.write(is_valid_world_set(worlds_folder_path_3_5_0_6))
    file.write(is_valid_world_set(worlds_folder_path_3_5_1_2))
