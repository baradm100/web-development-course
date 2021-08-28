# web-development-course

## Setup

```bash
# Clone the repo
git clone git@github.com:baradm100/web-development-course.git
cd web-development-course

# Install the ef tool
dotnet tool install --global dotnet-ef

# Install packages
dotnet restore
```

## Run Migrations

```bash
dotnet ef database update
```

## Add New Migration

```bash
dotnet ef migrations add InitialCreate
```

## Run In Watch Mode

```bash
dotnet watch run
```

## Documents

- [UML](https://lucid.app/lucidchart/invitations/accept/inv_d49a6e28-5842-4ef0-bf0c-35581ee00ad5)
