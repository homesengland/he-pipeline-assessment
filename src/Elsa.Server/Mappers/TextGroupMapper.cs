using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;

namespace Elsa.Server.Mappers
{
    public interface ITextGroupMapper
    {
        List<Information> InformationListFromGroupedTextModel(GroupedTextModel textModel);
        List<InformationTextGroup> InformationTextGroupListFromTextGroups(List<TextGroup> textGroups);
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
                InformationTextList = x.TextRecords.ConvertAll(y => new InformationText()
                {
                    Text = y.Text,
                    IsBold = y.IsBold,
                    IsParagraph = y.IsParagraph,
                    IsHyperlink = y.IsHyperlink,
                    Url = y.Url,

                })
            }).ToList();
        }

        public List<InformationTextGroup> InformationTextGroupListFromTextGroups(List<TextGroup> textGroups)
        {
            return textGroups.Select(x => new InformationTextGroup()
            {
                Title = x.Title,
                IsCollapsed = x.Collapsed,
                IsGuidance = x.Guidance,
                IsBullets = x.Bullets,
                InformationTextList = x.TextRecords.ConvertAll(y => new InformationText()
                {
                    Text = y.Text,
                    IsBold = y.IsBold,
                    IsParagraph = y.IsParagraph,
                    IsHyperlink = y.IsHyperlink,
                    Url = y.Url,

                })
            }).ToList();
        }
    }
}
