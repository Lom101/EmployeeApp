using FluentMigrator;

[Migration(1)]
public class M001_CreateTables : Migration
{
    public override void Up()
    {
        Execute.Sql("DROP TABLE IF EXISTS employees CASCADE;");
        Execute.Sql("DROP TABLE IF EXISTS departments CASCADE;");
        Execute.Sql("DROP TABLE IF EXISTS passports CASCADE;");

        Create.Table("passports")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("type").AsString(50).NotNullable()
            .WithColumn("number").AsString(50).NotNullable();

        Create.Table("departments")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("phone").AsString(20).NotNullable();

        Create.Table("employees")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("surname").AsString(100).NotNullable()
            .WithColumn("phone").AsString(100).NotNullable()
            .WithColumn("company_id").AsInt32().NotNullable()
            .WithColumn("passport_id").AsInt32().Nullable()
            .WithColumn("department_id").AsInt32().Nullable()
            .ForeignKey("fk_employee_passport", "passports", "id")
            .ForeignKey("fk_employee_department", "departments", "id");
    }

    public override void Down()
    {
        Delete.Table("employees");
        Delete.Table("departments");
        Delete.Table("passports");
    }
}