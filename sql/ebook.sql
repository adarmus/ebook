-- BOOKS


ALTER PROCEDURE [dbo].[spBook_INS]
    @id as uniqueidentifier,
    @title as nvarchar(200),
    @author as nvarchar(200),
    @isbn as nvarchar(20),
    @dateAdded as datetime,
    @publisher as nvarchar(200),
    @description as nvarchar(2000)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO [Book] 
    (
        [Id],
        [Title], 
        [Author], 
        [Isbn],
        [Publisher],
        [Description],
        [DateAdded]
    ) 
    VALUES 
    (
        @id,
        @title,
        @author,
        @isbn,
        @publisher,
        @description,
        @dateAdded
    )
END


ALTER PROCEDURE [dbo].[spBook_SEL_ALL]
AS
BEGIN
    SET NOCOUNT ON

    SELECT
        [Id],
        [Title], 
        [Author], 
        [Isbn],
        [Publisher],
        [Description],
        [DateAdded]
    FROM
        [book]
END



ALTER PROCEDURE [dbo].[spBook_SEL_BY_ID]
    @id as uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON

    SELECT
        [Id],
        [Title], 
        [Author], 
        [Isbn],
        [Publisher],
        [Description],
        [DateAdded]
    FROM
        [book]
    WHERE
        [Id] = @id
END



-- FILES


ALTER PROCEDURE [dbo].[spFile_INS]
    @id as uniqueidentifier,
    @bookid as uniqueidentifier,
    @fileType as nvarchar(50),
    @fileName as nvarchar(250),
    @content as varbinary(MAX)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO [File] 
    (
        [Id],
        [BookId], 
        [FileType], 
        [FileName],
        [Content]
    ) 
    VALUES 
    (
        @id,
        @bookid,
        @fileType,
        @fileName,
        @content
    )
END


CREATE PROCEDURE [dbo].[spFile_SEL_TYPE_BY_BOOKID]
    @bookid as uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON

    SELECT
        [Id],
        [BookId], 
        [FileType], 
        [FileName]
    FROM
        [file]
    WHERE
        [BookId] = @bookId
END
