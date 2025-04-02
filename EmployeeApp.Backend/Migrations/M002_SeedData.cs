using FluentMigrator;

[Migration(2)]
public class M002_SeedData : Migration
{
    public override void Up()
    {
        Insert.IntoTable("passports").Row(new { type = "Passport", number = "123456789" });
        Insert.IntoTable("passports").Row(new { type = "ID Card", number = "987654321" });

        Insert.IntoTable("departments").Row(new { name = "HR", phone = "123-456-7890" });
        Insert.IntoTable("departments").Row(new { name = "IT", phone = "987-654-3210" });
    }

    public override void Down()
    {
        Delete.FromTable("employees").AllRows();
        Delete.FromTable("departments").AllRows();
        Delete.FromTable("passports").AllRows();
    }
}