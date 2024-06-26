﻿using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;

namespace Elsa.Server.Mappers
{
    public interface ITextGroupMapper
    {
        List<Information> InformationListFromGroupedTextModel(GroupedTextModel textModel);
        List<Information> InformationListFromTextModel(TextModel textModel);
        List<InformationTextGroup> InformationTextGroupListFromTextGroupsForInformation(List<TextGroup> textGroups);
        List<InformationTextGroup> InformationTextGroupListFromTextGroupsForGuidance(List<TextGroup> textGroups);
        List<InformationTextGroup> InformationTextGroupListFromGuidanceString(string text);
    }

    public class TextGroupMapper : ITextGroupMapper
    {
        public List<Information> InformationListFromGroupedTextModel(GroupedTextModel textModel)
        {
            return textModel.TextGroups.Select(x => new Information()
            {
                Title = x.Title,
                IsCollapsed = x.Collapsed,
                IsGuidance = x.Guidance,
                IsBullets = x.Bullets,
                DisplayOnPage = x.DisplayOnPage,
                InformationTextList = x.TextRecords.ConvertAll(y => new InformationText()
                {
                    Text = y.Text,
                    IsBold = y.IsBold,
                    IsParagraph = y.IsParagraph,
                    IsHyperlink = y.IsHyperlink,
                    IsGuidance = y.IsGuidance,
                    Url = y.Url,

                })
            }).ToList();
        }

        public List<Information> InformationListFromTextModel(TextModel textModel)
        {
            return textModel.TextRecords.Select(x => new Information()
            {
                InformationTextList = new List<InformationText>()
                {
                    new InformationText()
                    {
                        Text = x.Text,
                        IsBold = x.IsBold,
                        IsParagraph = x.IsParagraph,
                        IsHyperlink = x.IsHyperlink,
                        IsGuidance = x.IsGuidance,
                        Url = x.Url,

                    }
                }}).ToList();
        }

        public List<InformationTextGroup> InformationTextGroupListFromTextGroupsForInformation(List<TextGroup> textGroups)
        {
            return textGroups.Select(x => new InformationTextGroup()
            {
                Title = x.Title,
                IsCollapsed = x.Collapsed,
                IsGuidance = x.Guidance,
                IsBullets = x.Bullets,
                DisplayOnPage = x.DisplayOnPage,
                InformationTextList = x.TextRecords.ConvertAll(y => new InformationText()
                {
                    Text = y.Text,
                    IsBold = y.IsBold,
                    IsParagraph = y.IsParagraph,
                    IsHyperlink = y.IsHyperlink,
                    IsGuidance = y.IsGuidance,
                    Url = y.Url,

                })
            }).ToList();
        }

        public List<InformationTextGroup> InformationTextGroupListFromTextGroupsForGuidance(List<TextGroup> textGroups)
        {
            return textGroups.Select(x => new InformationTextGroup()
            {
                Title = x.Title,
                IsCollapsed = false,
                IsGuidance = false,
                IsBullets = x.Bullets,
                DisplayOnPage = x.DisplayOnPage,
                InformationTextList = x.TextRecords.ConvertAll(y => new InformationText()
                {
                    Text = y.Text,
                    IsBold = y.IsBold,
                    IsParagraph = y.IsParagraph,
                    IsHyperlink = y.IsHyperlink,
                    IsGuidance = y.IsGuidance,
                    Url = y.Url,

                })
            }).ToList();
        }

        public List<InformationTextGroup> InformationTextGroupListFromGuidanceString(string text)
        {
            return new List<InformationTextGroup>()
            {
                new InformationTextGroup
                {
                    Title = "",
                    IsCollapsed = false,
                    IsGuidance = false,
                    IsBullets = false,
                    DisplayOnPage = true,
                    InformationTextList = new List<InformationText>()
                    {
                        new InformationText()
                        {
                            Text = text,
                            IsBold = false,
                            IsParagraph = true,
                            IsHyperlink = false,
                            Url = ""
                        }
                    }
                }
            };
        }
    }
}
