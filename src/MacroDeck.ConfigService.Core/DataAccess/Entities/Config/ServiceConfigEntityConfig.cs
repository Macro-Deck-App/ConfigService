using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeck.ConfigService.Core.DataAccess.Entities.Config;

public class ServiceConfigEntityConfig : BaseEntityConfig<ServiceConfigEntity>
{
    public ServiceConfigEntityConfig()
    {
        TableName = "config";
        ColumnPrefix = "cfg_";
    }

    public override void Configure(EntityTypeBuilder<ServiceConfigEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable(TableName, schema: Schema);
        
        builder.Property(x => x.Name)
            .HasColumnName(ColumnPrefix + "name")
            .IsRequired();

        builder.Property(x => x.ConfigValue)
            .HasColumnName(ColumnPrefix + "value");

        builder.Property(x => x.AccessTokenHash)
            .HasColumnName(ColumnPrefix + "access_token");

        builder.Property(x => x.AccessTokenSalt)
            .HasColumnName(ColumnPrefix + "access_token_salt");

        builder.Property(x => x.Version)
            .HasColumnName(ColumnPrefix + "version")
            .HasDefaultValue(1);
    }
}