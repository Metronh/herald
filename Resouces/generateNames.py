import csv
import random
import string

# Fixed and cleaned name lists
first_names = [
    "James", "Mary","John","Patricia","Robert","Jennifer","Michael","Linda","William","Elizabeth",
    "David","Susan","Richard","Jessica","Joseph","Sarah","Thomas","Karen","Charles","Nancy",
    "Christopher","Lisa","Daniel","Betty","Matthew","Ashley","Anthony","Sandra","Mark","Donna",
    "Paul","Dorothy","Steven","Emily","Andrew","Michelle","Joshua","Carol","Kevin","Amanda",
    "Brian","Melissa","George","Deborah","Timothy","Stephanie","Jason","Rebecca","Edward","Sharon",
    "Ronald","Laura","Nicholas","Cynthia","Ryan","Amy","Jacob","Kathleen","Gary","Shirley",
    "Eric","Helen","Jonathan","Brenda","Stephen","Pamela","Justin","Nicole","Scott","Anna",
    "Brandon","Samantha","Benjamin","Katherine","Samuel","Emma","Frank","Olivia","Gregory","Christine",
    "Alexander","Rachel","Patrick","Carolyn","Jack","Janet","Dennis","Catherine","Jerry","Ruth",
    "Tyler","Maria","Aaron","Heather","Henry","Diane","Douglas","Julie","Peter","Joyce"
]

last_names = [
    "Smith","Johnson","Williams","Brown","Jones","Garcia","Miller","Davis","Rodriguez","Martinez",
    "Hernandez","Lopez","Gonzalez","Wilson","Anderson","Thomas","Taylor","Moore","Jackson","Martin",
    "Lee","Perez","Thompson","White","Harris","Sanchez","Clark","Ramirez","Lewis","Robinson",
    "Walker","Young","Allen","King","Wright","Scott","Torres","Nguyen","Hill","Flores",
    "Green","Adams","Nelson","Baker","Hall","Rivera","Campbell","Mitchell","Carter","Roberts",
    "Gomez","Phillips","Evans","Turner","Diaz","Parker","Cruz","Edwards","Collins","Stewart",
    "Morris","Morales","Murphy","Cook","Rogers","Gutierrez","Ortiz","Morgan","Cooper","Peterson",
    "Bailey","Reed","Kelly","Howard","Ramos","Kim","Cox","Ward","Watson","Brooks"
]

# Characters for strong passwords
password_chars = string.ascii_letters + string.digits + "!@#$%^&*()_+-="

def generate_password(length=14):
    return ''.join(random.choice(password_chars) for _ in range(length))

# Generate 1000 unique users
users = []
used_emails = set()

random.seed(123)  # For consistent testing (remove if you want different each time)

while len(users) < 1000:
    first = random.choice(first_names)
    last = random.choice(last_names)
    
    # Realistic username variations
    username_style = random.choice([
        f"{first.lower()}{last.lower()}{random.randint(10, 9999)}",
        f"{first.lower()}_{last.lower()}{random.randint(1, 999)}",
        f"{first.lower()}{random.choice(['.', '_', ''])}{last.lower()}",
        f"{first[:3].lower()}{last.lower()}{random.randint(100, 9999)}"
    ])
    username = username_style

    # Ensure unique email
    base = f"{first.lower()}.{last.lower()}"
    email = f"{base}@example.com"
    counter = 1
    while email in used_emails:
        email = f"{base}{counter}@example.com"
        counter += 1
    used_emails.add(email)

    password = generate_password(14)

    users.append({
        "username": username,
        "email": email,
        "first_name": first,
        "last_name": last,
        "password": password
    })

# Write to CSV
with open('users_with_passwords.csv', 'w', newline='', encoding='utf-8') as file:
    writer = csv.DictWriter(file, fieldnames=['username', 'email', 'first_name', 'last_name', 'password'])
    writer.writeheader()
    writer.writerows(users)

print("1000 users generated successfully!")
print("Saved to: users_with_passwords.csv")
print("\nFirst 5 users:")
for user in users[:5]:
    print(f"{user['username']:<25} {user['email']:<35} {user['password']}")