using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeck.ConfigService.Core.DataAccess.Entities.Config;

public class UserEntityConfig : BaseEntityConfig<UserEntity>
{
    public UserEntityConfig()
    {
        TableName = "user";
        ColumnPrefix = "u_";
    }

    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable(TableName, schema: Schema);

        builder.Property(x => x.UserName)
            .HasColumnName(ColumnPrefix + "user_name")
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasColumnName(ColumnPrefix + "password_hash")
            .IsRequired();

        builder.Property(x => x.PasswordSalt)
            .HasColumnName(ColumnPrefix + "password_salt")
            .IsRequired();

        builder.Property(x => x.LastLogin)
            .HasColumnName(ColumnPrefix + "last_login");

        builder.Property(x => x.Role)
            .HasColumnName(ColumnPrefix + "role")
            .IsRequired();
    }
}