INSERT INTO [dbo].[InterventionReason]
([Name],[Order],[CreatedDateTime],[CreatedBy],[IsVariation])
VALUES
(''Project requires more money'',4,GETDATE(),''Admin'', 1),
(''Project is changing its strategic approach'',5,GETDATE(),''Admin'', 1),
(''Project is changing the numbers of units it would enable'',6,GETDATE(),''Admin'', 1),
(''Other (please specify)'',7,GETDATE(),''Admin'', 1)