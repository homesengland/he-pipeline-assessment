﻿using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.DataDictionaryHelpers;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Server.DataDictionaryAccessors;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Jint;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen.Helpers
{
    public class DataDictionaryHelperTests
    {
        [Theory, AutoMoqData]
        public async Task DataDictionaryValuesSetOnEngine_GivenCacheIsNotEmpty(
            [Frozen] Mock<IElsaCustomRepository> repository,
            [Frozen] Mock<IConnectionMultiplexer> cache,
            [Frozen] Mock<IDatabase> db,
            CancellationToken cancellationToken,
           [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
           Engine engine)
        {
            //Arrange
            List<DataDictionaryItem> cachedItems = new List<DataDictionaryItem>() {
                new DataDictionaryItem()
                {
                    Id = 1,
                    Group = "Group1",
                    Name = "Name1",
                },
                new DataDictionaryItem()
                {
                    Id = 2,
                    Group = "Group2",
                    Name = "Name2",
                },
                new DataDictionaryItem()
                {
                    Id = 3,
                    Group = "Group3",
                    Name = "Name3",
                },
            };
            var serialisedCachedItems = JsonConvert.SerializeObject(cachedItems);
            RedisValue redisValue = new RedisValue(serialisedCachedItems);
            cache.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);
            db.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(redisValue);
            CachedDataDictionaryIntellisenseAccessor accessor = new CachedDataDictionaryIntellisenseAccessor(cache.Object, repository.Object);
            DataDictionaryHelper sut = new DataDictionaryHelper(accessor);
            EvaluatingJavaScriptExpression notification = new EvaluatingJavaScriptExpression(engine, activityExecutionContext);

            //Act
            await sut.Handle(notification, cancellationToken);

            //Assert
            Assert.Equal(1, engine.GetValue("Group1_Name1"));
            Assert.Equal(2, engine.GetValue("Group2_Name2"));
            Assert.Equal(3, engine.GetValue("Group3_Name3"));
        }

        [Theory, AutoMoqData]
        public async Task DataDictionaryValuesSetOnEngine_GivenCacheIsEmpty(
        [Frozen] Mock<IElsaCustomRepository> repository,
        [Frozen] Mock<IConnectionMultiplexer> cache,
        [Frozen] Mock<IDatabase> db,
        CancellationToken cancellationToken,
        List<DataDictionary> dbDataDictionaryItems,
        [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
        Engine engine)
        {
            //Arrange
            List<DataDictionaryItem> cachedItems = new List<DataDictionaryItem>() {
                new DataDictionaryItem()
                {
                    Id = 1,
                    Group = "Group1",
                    Name = "Name1",
                },
                new DataDictionaryItem()
                {
                    Id = 2,
                    Group = "Group2",
                    Name = "Name2",
                },
                new DataDictionaryItem()
                {
                    Id = 3,
                    Group = "Group3",
                    Name = "Name3",
                },
            };
            var serialisedCachedItems = JsonConvert.SerializeObject(cachedItems);
            RedisValue nullRedisValue = new RedisValue(null);
            RedisValue redisValue = new RedisValue(serialisedCachedItems);
            repository.Setup(x => x.GetDataDictionaryListAsync(true, It.IsAny<CancellationToken>())).ReturnsAsync(dbDataDictionaryItems);
            cache.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);
            db.SetupSequence(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(nullRedisValue).ReturnsAsync(redisValue);
            CachedDataDictionaryIntellisenseAccessor accessor = new CachedDataDictionaryIntellisenseAccessor(cache.Object, repository.Object);
            DataDictionaryHelper sut = new DataDictionaryHelper(accessor);
            EvaluatingJavaScriptExpression notification = new EvaluatingJavaScriptExpression(engine, activityExecutionContext);

            //Act
            await sut.Handle(notification, cancellationToken);

            //Assert
            Assert.Equal(1, engine.GetValue("Group1_Name1"));
            Assert.Equal(2, engine.GetValue("Group2_Name2"));
            Assert.Equal(3, engine.GetValue("Group3_Name3"));
        }
    }
}
