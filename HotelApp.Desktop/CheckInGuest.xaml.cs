using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for CheckInGuest.xaml
    /// </summary>
    public partial class CheckInGuest : Window
    {
        private readonly IDatabaseData _db;
        private BookingFullModel _data = null;

        public CheckInGuest(IDatabaseData db)
        {
            InitializeComponent();

            _db = db;
        }

        public void PopulateCheckInInfo(BookingFullModel data)
        {
            _data = data;

            firstNameText.Text = _data.FirstName;
            lastNameText.Text = _data.LastName;
            titleText.Text = _data.Title;
            roomNumberText.Text = _data.RoomNumber;
            totalCostText.Text = String.Format("{0:C}", _data.TotalCost);
        }

        private void ConfirmCheckIn_Click(object sender, RoutedEventArgs e)
        {
            _db.CheckInGuest(_data.Id);

            Close();
        }
    }
}
