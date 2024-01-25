SET IDENTITY_INSERT DataDictionaryGroup ON  

INSERT INTO DataDictionaryGroup (Id, [Name], CreatedDateTime, LastModifiedDateTime)
SELECT Id, [Name], CreatedDateTime, LastModifiedDateTime
FROM QuestionDataDictionaryGroup

SET IDENTITY_INSERT DataDictionaryGroup OFF  



SET IDENTITY_INSERT DataDictionary ON  

INSERT INTO DataDictionary (Id, DataDictionaryGroupId, [Name], LegacyName, [Type], Description, CreatedDateTime, LastModifiedDateTime, IsArchived)
SELECT Id, QuestionDataDictionaryGroupId, [Name], LegacyName, [Type], Description, CreatedDateTime, LastModifiedDateTime, Coalesce(IsArchived, 0)
FROM QuestionDataDictionary

SET IDENTITY_INSERT DataDictionary OFF  


