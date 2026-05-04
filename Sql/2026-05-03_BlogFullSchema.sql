IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [BlogPosts] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Slug] nvarchar(200) NOT NULL,
    [Category] nvarchar(100) NULL,
    [Content] nvarchar(max) NOT NULL,
    [Excerpt] nvarchar(500) NULL,
    [FeaturedImageUrl] nvarchar(max) NULL,
    [Author] nvarchar(100) NOT NULL,
    [ReadTimeMinutes] int NOT NULL,
    [IsPublished] bit NOT NULL,
    [IsFeatured] bit NOT NULL,
    [PublishedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_BlogPosts] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CaseStudies] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Slug] nvarchar(200) NOT NULL,
    [Industry] nvarchar(100) NULL,
    [Services] nvarchar(200) NULL,
    [ResultMetric] nvarchar(200) NULL,
    [ResultDescription] nvarchar(500) NULL,
    [Content] nvarchar(max) NOT NULL,
    [ClientName] nvarchar(100) NULL,
    [FeaturedImageUrl] nvarchar(max) NULL,
    [IsPublished] bit NOT NULL,
    [IsFeatured] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_CaseStudies] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ContactSubmissions] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(100) NOT NULL,
    [Email] nvarchar(200) NOT NULL,
    [Company] nvarchar(100) NULL,
    [Subject] nvarchar(200) NULL,
    [Message] nvarchar(max) NOT NULL,
    [IsRead] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ContactSubmissions] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PageContents] (
    [Id] int NOT NULL IDENTITY,
    [PageSlug] nvarchar(100) NOT NULL,
    [SectionKey] nvarchar(100) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [ContentType] nvarchar(50) NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_PageContents] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [SiteSettings] (
    [Id] int NOT NULL IDENTITY,
    [SiteName] nvarchar(200) NOT NULL,
    [SiteDescription] nvarchar(500) NULL,
    [ContactEmail] nvarchar(200) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [Address] nvarchar(500) NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_SiteSettings] PRIMARY KEY ([Id])
);
GO

CREATE UNIQUE INDEX [IX_BlogPosts_Slug] ON [BlogPosts] ([Slug]);
GO

CREATE UNIQUE INDEX [IX_CaseStudies_Slug] ON [CaseStudies] ([Slug]);
GO

CREATE INDEX [IX_ContactSubmissions_Email] ON [ContactSubmissions] ([Email]);
GO

CREATE INDEX [IX_ContactSubmissions_IsRead] ON [ContactSubmissions] ([IsRead]);
GO

CREATE UNIQUE INDEX [IX_PageContents_PageSlug_SectionKey] ON [PageContents] ([PageSlug], [SectionKey]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260330102447_InitialCreate', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [BlogPosts] ADD [BlogAuthorId] int NULL;
GO

ALTER TABLE [BlogPosts] ADD [BlogCategoryId] int NULL;
GO

ALTER TABLE [BlogPosts] ADD [CanonicalUrl] nvarchar(500) NULL;
GO

ALTER TABLE [BlogPosts] ADD [FeaturedImageAlt] nvarchar(200) NULL;
GO

ALTER TABLE [BlogPosts] ADD [FeaturedImageHeight] int NULL;
GO

ALTER TABLE [BlogPosts] ADD [FeaturedImageWidth] int NULL;
GO

ALTER TABLE [BlogPosts] ADD [MetaDescription] nvarchar(500) NULL;
GO

ALTER TABLE [BlogPosts] ADD [MetaTitle] nvarchar(200) NULL;
GO

ALTER TABLE [BlogPosts] ADD [WordCount] int NULL;
GO

CREATE TABLE [BlogAuthors] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Slug] nvarchar(120) NOT NULL,
    [Bio] nvarchar(1000) NULL,
    [AvatarUrl] nvarchar(500) NULL,
    [Role] nvarchar(100) NULL,
    [TwitterUrl] nvarchar(500) NULL,
    [LinkedinUrl] nvarchar(500) NULL,
    [InstagramUrl] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_BlogAuthors] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [BlogCategories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Slug] nvarchar(120) NOT NULL,
    [Description] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_BlogCategories] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [BlogTags] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(80) NOT NULL,
    [Slug] nvarchar(100) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_BlogTags] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [BlogPostTags] (
    [BlogPostId] int NOT NULL,
    [BlogTagId] int NOT NULL,
    CONSTRAINT [PK_BlogPostTags] PRIMARY KEY ([BlogPostId], [BlogTagId]),
    CONSTRAINT [FK_BlogPostTags_BlogPosts_BlogPostId] FOREIGN KEY ([BlogPostId]) REFERENCES [BlogPosts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BlogPostTags_BlogTags_BlogTagId] FOREIGN KEY ([BlogTagId]) REFERENCES [BlogTags] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_BlogPosts_BlogAuthorId] ON [BlogPosts] ([BlogAuthorId]);
GO

CREATE INDEX [IX_BlogPosts_BlogCategoryId] ON [BlogPosts] ([BlogCategoryId]);
GO

CREATE UNIQUE INDEX [IX_BlogAuthors_Slug] ON [BlogAuthors] ([Slug]);
GO

CREATE UNIQUE INDEX [IX_BlogCategories_Slug] ON [BlogCategories] ([Slug]);
GO

CREATE INDEX [IX_BlogPostTags_BlogTagId] ON [BlogPostTags] ([BlogTagId]);
GO

CREATE UNIQUE INDEX [IX_BlogTags_Slug] ON [BlogTags] ([Slug]);
GO

ALTER TABLE [BlogPosts] ADD CONSTRAINT [FK_BlogPosts_BlogAuthors_BlogAuthorId] FOREIGN KEY ([BlogAuthorId]) REFERENCES [BlogAuthors] ([Id]) ON DELETE SET NULL;
GO

ALTER TABLE [BlogPosts] ADD CONSTRAINT [FK_BlogPosts_BlogCategories_BlogCategoryId] FOREIGN KEY ([BlogCategoryId]) REFERENCES [BlogCategories] ([Id]) ON DELETE SET NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260503085259_AddBlogAuthorsCategoriesAndTags', N'8.0.0');
GO

COMMIT;
GO

