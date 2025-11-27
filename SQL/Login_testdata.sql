-- Vložení testovacího uživatele do tabulky Login
USE [Volejbal]
GO

-- Smazání existujících záznamů (volitelné)
-- DELETE FROM [dbo].[Login]

-- Vložení testovacích uživatelů
INSERT INTO [dbo].[Login] ([jmeno], [prijmeni], [heslo])
VALUES 
    ('Admin', 'Adminovic', 'admin123'),
    ('Michal', 'Krizak', 'heslo123'),
    ('Test', 'User', 'test123')
GO

-- Ověření vložených dat
SELECT * FROM [dbo].[Login]
GO
