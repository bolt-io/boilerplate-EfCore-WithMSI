using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EFCore.Data.Config
{
    public sealed class SampleEntityConfiguration : IEntityTypeConfiguration<SampleEntity>
    {
        public void Configure(EntityTypeBuilder<SampleEntity> builder)
        {
            builder.ToTable(name: "Entities",schema: "sample")
                    .HasKey(x=>x.Id);
            
            builder.Property(x=>x.ExampleProperty)
                    .HasMaxLength(120)
                    .HasComment("This is an example of a column comment.");
                    
            

        }
    }
}
