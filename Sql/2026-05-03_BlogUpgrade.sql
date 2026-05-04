/*
================================================================================
 Blog framework upgrade — Toro Solutions
 Date: 2026-05-03
 Adds support for the rich blog format consumed by the public Toro Solutions FE
 and published into by Verqos.

 What this script does:
   1. Creates new tables: BlogAuthors, BlogCategories, BlogTags, BlogPostTags
   2. Adds new columns to BlogPosts: BlogAuthorId, BlogCategoryId, MetaTitle,
      MetaDescription, CanonicalUrl, WordCount, FeaturedImageAlt/Width/Height
   3. Adds indexes and FKs (with ON DELETE SET NULL on author/category)
   4. Backfills:
        - Inserts default "Chris Paton" author and links existing posts to it
        - Creates BlogCategory rows from distinct legacy Category strings and
          links existing posts to them

 Idempotent — safe to re-run. Each section guards against repeated execution.
 Target: SQL Server.
================================================================================
*/

SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- ============================================================================
-- 1. Schema: new tables
-- ============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BlogAuthors')
BEGIN
    CREATE TABLE [BlogAuthors] (
        [Id] INT NOT NULL IDENTITY,
        [Name] NVARCHAR(100) NOT NULL,
        [Slug] NVARCHAR(120) NOT NULL,
        [Bio] NVARCHAR(1000) NULL,
        [AvatarUrl] NVARCHAR(500) NULL,
        [Role] NVARCHAR(100) NULL,
        [TwitterUrl] NVARCHAR(500) NULL,
        [LinkedinUrl] NVARCHAR(500) NULL,
        [InstagramUrl] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [UpdatedAt] DATETIME2 NOT NULL,
        CONSTRAINT [PK_BlogAuthors] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_BlogAuthors_Slug] ON [BlogAuthors] ([Slug]);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BlogCategories')
BEGIN
    CREATE TABLE [BlogCategories] (
        [Id] INT NOT NULL IDENTITY,
        [Name] NVARCHAR(100) NOT NULL,
        [Slug] NVARCHAR(120) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [UpdatedAt] DATETIME2 NOT NULL,
        CONSTRAINT [PK_BlogCategories] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_BlogCategories_Slug] ON [BlogCategories] ([Slug]);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BlogTags')
BEGIN
    CREATE TABLE [BlogTags] (
        [Id] INT NOT NULL IDENTITY,
        [Name] NVARCHAR(80) NOT NULL,
        [Slug] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        CONSTRAINT [PK_BlogTags] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_BlogTags_Slug] ON [BlogTags] ([Slug]);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BlogPostTags')
BEGIN
    CREATE TABLE [BlogPostTags] (
        [BlogPostId] INT NOT NULL,
        [BlogTagId] INT NOT NULL,
        CONSTRAINT [PK_BlogPostTags] PRIMARY KEY ([BlogPostId], [BlogTagId]),
        CONSTRAINT [FK_BlogPostTags_BlogPosts_BlogPostId]
            FOREIGN KEY ([BlogPostId]) REFERENCES [BlogPosts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BlogPostTags_BlogTags_BlogTagId]
            FOREIGN KEY ([BlogTagId]) REFERENCES [BlogTags] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_BlogPostTags_BlogTagId] ON [BlogPostTags] ([BlogTagId]);
END
GO

-- ============================================================================
-- 2. Schema: new columns on BlogPosts
-- ============================================================================

IF COL_LENGTH('BlogPosts', 'BlogAuthorId') IS NULL
    ALTER TABLE [BlogPosts] ADD [BlogAuthorId] INT NULL;
GO

IF COL_LENGTH('BlogPosts', 'BlogCategoryId') IS NULL
    ALTER TABLE [BlogPosts] ADD [BlogCategoryId] INT NULL;
GO

IF COL_LENGTH('BlogPosts', 'CanonicalUrl') IS NULL
    ALTER TABLE [BlogPosts] ADD [CanonicalUrl] NVARCHAR(500) NULL;
GO

IF COL_LENGTH('BlogPosts', 'FeaturedImageAlt') IS NULL
    ALTER TABLE [BlogPosts] ADD [FeaturedImageAlt] NVARCHAR(200) NULL;
GO

IF COL_LENGTH('BlogPosts', 'FeaturedImageHeight') IS NULL
    ALTER TABLE [BlogPosts] ADD [FeaturedImageHeight] INT NULL;
GO

IF COL_LENGTH('BlogPosts', 'FeaturedImageWidth') IS NULL
    ALTER TABLE [BlogPosts] ADD [FeaturedImageWidth] INT NULL;
GO

IF COL_LENGTH('BlogPosts', 'MetaDescription') IS NULL
    ALTER TABLE [BlogPosts] ADD [MetaDescription] NVARCHAR(500) NULL;
GO

IF COL_LENGTH('BlogPosts', 'MetaTitle') IS NULL
    ALTER TABLE [BlogPosts] ADD [MetaTitle] NVARCHAR(200) NULL;
GO

IF COL_LENGTH('BlogPosts', 'WordCount') IS NULL
    ALTER TABLE [BlogPosts] ADD [WordCount] INT NULL;
GO

-- ============================================================================
-- 3. Schema: indexes and foreign keys on BlogPosts
-- ============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BlogPosts_BlogAuthorId' AND object_id = OBJECT_ID('BlogPosts'))
    CREATE INDEX [IX_BlogPosts_BlogAuthorId] ON [BlogPosts] ([BlogAuthorId]);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BlogPosts_BlogCategoryId' AND object_id = OBJECT_ID('BlogPosts'))
    CREATE INDEX [IX_BlogPosts_BlogCategoryId] ON [BlogPosts] ([BlogCategoryId]);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BlogPosts_BlogAuthors_BlogAuthorId')
    ALTER TABLE [BlogPosts] ADD CONSTRAINT [FK_BlogPosts_BlogAuthors_BlogAuthorId]
        FOREIGN KEY ([BlogAuthorId]) REFERENCES [BlogAuthors] ([Id]) ON DELETE SET NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BlogPosts_BlogCategories_BlogCategoryId')
    ALTER TABLE [BlogPosts] ADD CONSTRAINT [FK_BlogPosts_BlogCategories_BlogCategoryId]
        FOREIGN KEY ([BlogCategoryId]) REFERENCES [BlogCategories] ([Id]) ON DELETE SET NULL;
GO

-- ============================================================================
-- 4. Data backfill: default author
-- ============================================================================

DECLARE @ChrisAuthorId INT;

SELECT @ChrisAuthorId = [Id] FROM [BlogAuthors] WHERE [Slug] = 'chris-paton';

IF @ChrisAuthorId IS NULL
BEGIN
    INSERT INTO [BlogAuthors] ([Name], [Slug], [Bio], [Role], [AvatarUrl], [CreatedAt], [UpdatedAt])
    VALUES (
        N'Chris Paton',
        N'chris-paton',
        N'Founder of Toro Solutions. Strategist and technologist focused on AI, data, and digital transformation.',
        N'Founder',
        N'/images/authors/chris-paton.jpg',
        SYSUTCDATETIME(),
        SYSUTCDATETIME()
    );
    SET @ChrisAuthorId = SCOPE_IDENTITY();
END

-- Link any existing posts that match the legacy Author string
UPDATE [BlogPosts]
SET [BlogAuthorId] = @ChrisAuthorId
WHERE [BlogAuthorId] IS NULL
  AND [Author] = N'Chris Paton';
GO

-- ============================================================================
-- 5. Data backfill: categories
-- ============================================================================

-- Insert a BlogCategory row for each distinct legacy Category string used by posts
INSERT INTO [BlogCategories] ([Name], [Slug], [CreatedAt], [UpdatedAt])
SELECT DISTINCT
    bp.[Category] AS [Name],
    LOWER(
        REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
            LTRIM(RTRIM(bp.[Category])),
            '&', 'and'),
            '''', ''),
            '"', ''),
            '(', ''),
            ')', ''),
            ',', ''),
            ':', ''),
            '/', '-'),
            ' ', '-'),
            '--', '-')
    ) AS [Slug],
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
FROM [BlogPosts] bp
WHERE bp.[Category] IS NOT NULL
  AND bp.[Category] <> ''
  AND NOT EXISTS (
      SELECT 1 FROM [BlogCategories] bc
      WHERE bc.[Slug] = LOWER(
          REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
              LTRIM(RTRIM(bp.[Category])),
              '&', 'and'),
              '''', ''),
              '"', ''),
              '(', ''),
              ')', ''),
              ',', ''),
              ':', ''),
              '/', '-'),
              ' ', '-'),
              '--', '-')
      )
  );
GO

-- Link any existing posts to their category by matching slug
UPDATE bp
SET bp.[BlogCategoryId] = bc.[Id]
FROM [BlogPosts] bp
INNER JOIN [BlogCategories] bc
    ON bc.[Slug] = LOWER(
        REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
            LTRIM(RTRIM(bp.[Category])),
            '&', 'and'),
            '''', ''),
            '"', ''),
            '(', ''),
            ')', ''),
            ',', ''),
            ':', ''),
            '/', '-'),
            ' ', '-'),
            '--', '-')
    )
WHERE bp.[BlogCategoryId] IS NULL
  AND bp.[Category] IS NOT NULL
  AND bp.[Category] <> '';
GO

-- ============================================================================
-- 6. EF migration history (only if __EFMigrationsHistory exists)
--    Skipped if the project doesn't use EF migration tracking.
--    Prevents Database.Migrate() from trying to re-apply this change.
-- ============================================================================

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM [__EFMigrationsHistory]
        WHERE [MigrationId] = N'20260503085259_AddBlogAuthorsCategoriesAndTags'
    )
    BEGIN
        INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
        VALUES (N'20260503085259_AddBlogAuthorsCategoriesAndTags', N'8.0.0');
    END
END
GO

COMMIT TRANSACTION;
GO

PRINT 'Blog framework upgrade applied successfully.';
GO
