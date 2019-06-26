-- script para criar a tabela ArquivoPlaiAnexo

CREATE TABLE ArquivoPlaiAnexo
	(
	IdArquivoPlaiAnexo int NOT NULL IDENTITY (1, 1),
	IdPlai int NOT NULL,
	IdAnexo int NOT NULL
	)  
GO
ALTER TABLE ArquivoPlaiAnexo ADD CONSTRAINT
	PK_ArquivoPlaiAnexo PRIMARY KEY CLUSTERED 
	(
	IdArquivoPlaiAnexo
	) 

GO

ALTER TABLE dbo.ArquivoPlaiAnexo ADD CONSTRAINT
	FK_ArquivoPlaiAnexo_Anexo FOREIGN KEY
	(
	IdAnexo
	) REFERENCES dbo.Anexo
	(
	IdAnexo
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE
	
GO
ALTER TABLE dbo.ArquivoPlaiAnexo ADD CONSTRAINT
	FK_ArquivoPlaiAnexo_Plai FOREIGN KEY
	(
	IdPlai
	) REFERENCES dbo.Plai
	(
	IdPlai
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO




--colunas que nao usamos
--deixamos ela por enquanto por dificuldade na implantação, mas elas deverão ser removidas sim! é que quem está em outra branch nao vai conseguir rodar contra o banco de dados sem essas colunas!
GO
ALTER TABLE [Plai] DROP COLUMN [IdArquivo]
ALTER TABLE [Plai] DROP COLUMN [Arquivo_IdAnexo]

GO

