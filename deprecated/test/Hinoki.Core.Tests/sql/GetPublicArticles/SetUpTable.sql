USE [blog]
GO

/****** オブジェクト: Table [dbo].[articles] スクリプト日付: 2014/08/23 20:07:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[articles];


GO
CREATE TABLE [dbo].[articles] (
    [id]            INT            IDENTITY (1, 1) NOT NULL,
    [insert_date]   DATETIME       NOT NULL,
    [update_date]   DATETIME       NOT NULL,
    [release_date]  DATETIME       NOT NULL,
    [title]         NVARCHAR (50)  NOT NULL,
    [body]          NVARCHAR (MAX) NULL,
    [public_status] INT            NOT NULL
);


