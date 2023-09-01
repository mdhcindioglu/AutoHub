using AutoHub.Client.Pages.Dialogs;
using AutoHub.Shared.Entities;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace AutoHub.Client.Pages
{
    public partial class Index
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        bool loading;
        public bool Loading { get => loading; set { loading = value; StateHasChanged(); } }
        List<Step>? Steps { get; set; }
        List<Item>? Items { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await FillSteps();
        }

        async Task FillSteps()
        {
            Loading = true;

            var response = await StepSrv.Steps();
            if (response != null)
            {
                if (response.IsSuccess)
                {
                    Steps = response.Data;
                    if (Steps?.Any() == true)
                    {
                        Steps.First().Selected = true;
                        await FillItems();
                    }
                }
                else
                    ToastSrv.ShowError(response.GetErrors);
            }

            Loading = false;
        }

        async Task AddStep()
        {
            var stepModal = Modal.Show<StepDialog>("Create new step", new ModalParameters { { nameof(Step), new Step() } }, new ModalOptions { });
            var result = await stepModal.Result;
            if (result.Confirmed)
                await FillSteps();
        }

        async Task UpdateStep(Step step)
        {
            var stepModal = Modal.Show<StepDialog>("Update step", new ModalParameters { { nameof(Step), step } }, new ModalOptions { });
            var result = await stepModal.Result;
            if (result.Confirmed)
                await FillSteps();
        }

        async Task DeleteStep(Step step)
        {
            if (await JS.Confirmation($"Are you sure to delete the step [{step.Title}] and all items?", "Delete Step"))
            {
                Loading = true;

                var response = await StepSrv.Delete(step.Id);
                if (response != null)
                {
                    if (response.IsSuccess)
                        await FillSteps();
                    else
                        ToastSrv.ShowError(response.GetErrors);
                }

                Loading = false;
            }
        }

        async Task FillItems()
        {
            Loading = true;

            var id = Steps?.FirstOrDefault(x => x.Selected)?.Id ?? 0;
            if (id > 0)
            {
                var response = await ItemSrv.Items(id);
                if (response != null)
                {
                    if (response.IsSuccess)
                        Items = response.Data;
                    else
                        ToastSrv.ShowError(response.GetErrors);
                }
            }

            Loading = false;
        }
        async Task AddItem()
        {
            var itemModal = Modal.Show<ItemDialog>("Create new item", new ModalParameters { { nameof(Item), new Item() { StepId = Steps?.FirstOrDefault(x => x.Selected)?.Id ?? 0 } } }, new ModalOptions { });
            var result = await itemModal.Result;
            if (result.Confirmed)
                await FillItems();
        }

        async Task UpdateItem(Item item)
        {
            var itemModal = Modal.Show<ItemDialog>("Update item", new ModalParameters { { nameof(Item), item } }, new ModalOptions { });
            var result = await itemModal.Result;
            if (result.Confirmed)
                await FillItems();
        }

        async Task DeleteItem(Item item)
        {
            if (await JS.Confirmation($"Are you sure to delete the item [{item.Title}]?", "Delete Item"))
            {
                Loading = true;

                var response = await ItemSrv.Delete(item.Id);
                if (response != null)
                {
                    if (response.IsSuccess)
                        await FillItems();
                    else
                        ToastSrv.ShowError(response.GetErrors);
                }

                Loading = false;
            }
        }

        async Task Next()
        {
            if (Steps != null)
            {
                for (int i = 0; i < Steps.Count; i++)
                {
                    if (Steps[Steps.Count - 1].Selected) return;
                    if (Steps[i].Selected)
                    {
                        Steps[i].Selected = false;
                        Steps[i + 1].Selected = true;
                        await FillItems();
                        return;
                    }
                }
            }
        }

        async Task Prev()
        {
            if (Steps != null)
            {
                for (int i = Steps.Count - 1; i >= 0; i--)
                {
                    if (Steps[0].Selected) return;
                    if (Steps[i].Selected)
                    {
                        Steps[i].Selected = false;
                        Steps[i - 1].Selected = true;
                        await FillItems();
                        return;
                    }
                }
            }
        }
    }
}
