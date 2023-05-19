using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeck.ConfigService.Core.DataAccess.Entities.Config;

public abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    protected const string Schema = "configservice";
    public required string TableName { get; set; }
    public required string ColumnPrefix { get; set; }
    
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.CreatedTimestamp)
            .HasColumnName("created_timestamp")
            .IsRequired();

        builder.Property(x => x.UpdatedTimestamp)
            .HasColumnName("updated_timestamp");
    }
}