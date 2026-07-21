import yaml
import collections
import os

path = r'Assets\Scripts\Data\MasterDatingPool.asset'
if not os.path.exists(path):
    print(f"File not found: {path}")
    exit(1)

with open(path, 'r') as f:
    content = f.read()
    # Unity YAML starts with %YAML and has multiple documents
    # The actual data is in the MonoBehavior document
    docs = content.split('---')
    data = None
    for doc in docs:
        if 'MonoBehaviour:' in doc:
            # Strip Unity specific tags for safe_load if necessary, 
            # but usually yaml can handle it if we are careful or use a simpler parser
            # Actually, safe_load might fail on !u! tag. 
            # Let's use a simpler line-by-line approach to be safe if yaml fails.
            pass

# Simpler line-by-line counting to avoid YAML parsing issues with Unity tags
rel_counts = collections.Counter()
smoker_counts = collections.Counter()
pet_counts = collections.Counter()
total_profiles = 0

rel_names = {0: 'LongTerm', 1: 'Casual', 2: 'Friendship'}
smoker_names = {0: 'Smoker', 1: 'NonSmoker'}
pet_names = {0: 'HasPets', 1: 'NoPets'}

lines = content.splitlines()
for i, line in enumerate(lines):
    if line.strip() == '-': # Start of a new profile element
        total_profiles += 1
    if 'relationshipType:' in line:
        val = int(line.split(':')[-1].strip())
        rel_counts[val] += 1
    if 'smokerStatus:' in line:
        val = int(line.split(':')[-1].strip())
        smoker_counts[val] += 1
    if 'petStatus:' in line:
        val = int(line.split(':')[-1].strip())
        pet_counts[val] += 1

print('Total Profiles:', total_profiles)
print('\nRelationship Type counts:')
for k in range(3):
    print(f'  {rel_names[k]}: {rel_counts[k]}')

print('\nSmoker Status counts:')
for k in range(2):
    print(f'  {smoker_names[k]}: {smoker_counts[k]}')
    
print('\nPet Status counts:')
for k in range(2):
    print(f'  {pet_names[k]}: {pet_counts[k]}')
