using Client.BussinesModels;
using Client.ViewModels;
using Database.Enums;
using System.Collections.Generic;

namespace Client.Commands
{
    public class UpdateView
    {
        #region Declaration

        private MainViewModel mainViewModel;
        private SearchCarViewModel searchCarViewModel;
        private SelectCarViewModel selectCarViewModel;
        private SelectedCarViewModel selectedCarViewModel;
        private ClientInformationViewModel clientInformationViewModel;
        private SuccessfulReservationViewModel successfulReservationViewModel;
        private NotFoundCarsViewModel notFoundCarsViewModel;

        #endregion

        #region Constructor

        public UpdateView(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            mainViewModel.ReturnToHomeEvent += ReturnToHomeHandler;
            searchCarViewModel = new SearchCarViewModel();
            searchCarViewModel.SearchEvent += SearchHandler;
            mainViewModel.SelectedViewModel = searchCarViewModel;

        }

        #endregion

        #region Methods

        public void ReturnToHomeHandler()
        {
            searchCarViewModel = new SearchCarViewModel();
            mainViewModel.SelectedViewModel = searchCarViewModel;
            searchCarViewModel.SearchEvent += SearchHandler;
        }

        public void ReturnToSelectHandler(CarPeriodWrapper carPeriodWrapper, CarType carType)
        {
            selectCarViewModel = new SelectCarViewModel(carPeriodWrapper, carType);
            mainViewModel.SelectedViewModel = selectCarViewModel;
            selectCarViewModel.SelectEvent += SelectHandler;
        }

        public void SearchHandler(CarPeriodWrapper carPeriodWrapper, List<CarPeriodWrapper> cars, CarType carType)
        {
            if(cars.Count != 0)
            {
                selectCarViewModel = new SelectCarViewModel(carPeriodWrapper, carType);
                mainViewModel.SelectedViewModel = selectCarViewModel;
                selectCarViewModel.SelectEvent += SelectHandler;
            }
            else
            {
                notFoundCarsViewModel = new NotFoundCarsViewModel();
                mainViewModel.SelectedViewModel = notFoundCarsViewModel;
            }
           
        }

        public void SelectHandler(CarPeriodWrapper carPeriodWrapper)
        {
            selectedCarViewModel = new SelectedCarViewModel(carPeriodWrapper);
            mainViewModel.SelectedViewModel = selectedCarViewModel;
            selectedCarViewModel.SelectedEvent += SelectedHandler;
            selectedCarViewModel.ReturnToSelectEvent += ReturnToSelectHandler;
        }

        public void SelectedHandler(CarPeriodWrapper carPeriodWrapper)
        {
            clientInformationViewModel = new ClientInformationViewModel(carPeriodWrapper);
            mainViewModel.SelectedViewModel = clientInformationViewModel;
            clientInformationViewModel.ChangeViewEvent += ClientInformtionHandler;
            clientInformationViewModel.ReturnToSelectEvent += ReturnToSelectHandler;
        }

        public void ClientInformtionHandler()
        {
            successfulReservationViewModel = new SuccessfulReservationViewModel();
            mainViewModel.SelectedViewModel = successfulReservationViewModel;
        }

        #endregion
    }
}
