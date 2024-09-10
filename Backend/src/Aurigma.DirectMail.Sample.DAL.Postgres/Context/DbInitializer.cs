using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Aurigma.DirectMail.Sample.DAL.Postgres.Context;

public class DbInitializer(ModelBuilder modelBuilder)
{
    private readonly ModelBuilder _modelBuilder = modelBuilder;

    public void Seed()
    {
        _modelBuilder
            .Entity<CompanyDal>()
            .HasData(
                new CompanyDal
                {
                    Id = new Guid("28394c53-2b8a-48c3-96de-b0c066b3aa9a"),
                    Name = "Aurigma",
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                },
                new CompanyDal
                {
                    Id = new Guid("6e3f0333-01e9-4e74-8a2f-bbe1e2d5b5fc"),
                    Name = "Some Company",
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                }
            );

        _modelBuilder
            .Entity<CategoryDal>()
            .HasData(
                new CategoryDal
                {
                    Id = new Guid("f95777aa-16e5-4e49-a464-b86139e861ab"),
                    Title = "Postcards",
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                },
                new CategoryDal
                {
                    Id = new Guid("cfd22d2b-cf98-4c72-8992-c791790c90bd"),
                    Title = "Letters",
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                },
                new CategoryDal
                {
                    Id = new Guid("dd4d7f75-a6a7-45d2-a74e-d2d686719e96"),
                    Title = "Selfmailers",
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                }
            );

        _modelBuilder
            .Entity<ProductDal>()
            .HasData(
                new ProductDal
                {
                    Id = new Guid("e5f8b300-83e7-4754-af1e-86a9946e303e"),
                    Title = "Folded Card",
                    Price = 55.9m,
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                    CategoryId = new Guid("f95777aa-16e5-4e49-a464-b86139e861ab"),
                },
                new ProductDal
                {
                    Id = new Guid("d7f28121-9b3a-4927-87f7-97a6f9c79381"),
                    Title = "Selfmailer",
                    Price = 65.9m,
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                    CategoryId = new Guid("dd4d7f75-a6a7-45d2-a74e-d2d686719e96"),
                },
                new ProductDal
                {
                    Id = new Guid("8b844daf-a3cf-4b4c-9758-79404b903379"),
                    Title = "Letter",
                    Price = 55.9m,
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                    CategoryId = new Guid("cfd22d2b-cf98-4c72-8992-c791790c90bd"),
                },
                new ProductDal
                {
                    Id = new Guid("fe9971da-6721-478c-8256-5d10951ee7f8"),
                    Title = "Postcard",
                    Price = 65.9m,
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                    CategoryId = new Guid("f95777aa-16e5-4e49-a464-b86139e861ab"),
                }
            );

        _modelBuilder
            .Entity<RecipientListDal>()
            .HasData(
                new RecipientListDal
                {
                    Id = new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                    Title = "Recipients",
                    CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                }
            );

        var images = ImportImagesFromAppDataFolder();
        var recipients = ImportRecipientsFromJsonFile();

        _modelBuilder.Entity<RecipientImageDal>().HasData(images);
        _modelBuilder.Entity<RecipientDal>().HasData(recipients);
        _modelBuilder
            .Entity<RecipientImageRecipientDal>()
            .HasData(ImportRecipientImageRecipientFromJsonFile());
    }

    private List<RecipientDal> ImportRecipientsFromJsonFile()
    {
        var assemblyLocation = AppDomain.CurrentDomain.BaseDirectory;
        var recipientJson = System.IO.File.ReadAllText(
            System.IO.Path.Combine(assemblyLocation, "App_Data/Recipients" + "/recipients.json")
        );
        var recipients = JsonConvert.DeserializeObject<List<RecipientDal>>(recipientJson);

        return recipients;
    }

    private List<RecipientImageDal> ImportImagesFromAppDataFolder()
    {
        var assemblyLocation = AppDomain.CurrentDomain.BaseDirectory;
        var imagePaths = System.IO.Directory.EnumerateFiles(
            System.IO.Path.Combine(assemblyLocation, "App_Data/PrivateImages")
        );
        var images = new List<RecipientImageDal>();
        foreach (var imagePath in imagePaths)
        {
            var relativePath = System.IO.Path.GetRelativePath(assemblyLocation, imagePath);
            var formattedFilePath = relativePath.Replace('\\', '/');
            var fileName = System.IO.Path.GetFileName(formattedFilePath);

            // Compute file name hash for generate GUID
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(fileName));

            var image = new RecipientImageDal
            {
                Id = new Guid(hash),
                CreationTime = new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                Path = formattedFilePath,
                Name = fileName,
            };

            images.Add(image);
        }

        return images;
    }

    private List<RecipientImageRecipientDal> ImportRecipientImageRecipientFromJsonFile()
    {
        var assemblyLocation = AppDomain.CurrentDomain.BaseDirectory;
        var recipientImageRecipientJson = System.IO.File.ReadAllText(
            System.IO.Path.Combine(
                assemblyLocation,
                "App_Data/Recipients" + "/recipientImageRecipient.json"
            )
        );
        var recipientImageRecipients = JsonConvert.DeserializeObject<
            List<RecipientImageRecipientDal>
        >(recipientImageRecipientJson);

        return recipientImageRecipients;
    }
}
