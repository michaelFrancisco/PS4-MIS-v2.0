﻿using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PS4_MIS_v2._0.ViewModels.CriminalRecords
{
    class EditCriminalRecordViewModel : Screen
    {
        private IWindowManager windowManager = new WindowManager();
        private DateTime _birthdate;
        private DateTime _dateofarrest;
        private DateTime _birthdateSelectedDate;
        private DateTime _dateofarrestSelectedDate;
        private List<string> _sex;
        private string _criminalID;
        private string _age;
        private string _criminalPicture = null;
        private string _remarks;
        private string _haircolor;
        private string _eyecolor;
        private string _arrestingofficer;
        private string _placeofarrest;
        private string _crime;
        private string _address;
        private string _birthplace;
        private string _selectedSex;
        private string _lastname;
        private string _middlename;
        private string _firstname;

        private static string _criminalPictureFilePath;
        private static string _savedCriminalPictureFilePath;
        private ImageSource _criminalPictureSource;
        private bool _hasPicture = false;

        private int _selectedCriminalID;

        public EditCriminalRecordViewModel(int selectedCriminalID)
        {
            _selectedCriminalID = selectedCriminalID;
        }

        protected override void OnActivate()
        {
            initializeValues();            
            base.OnActivate();
        }

        public void initializeValues()
        {
            DataTable dt = connection.dbTable("select * from criminalrecords where Criminal_ID =" + _selectedCriminalID + ";");
            _criminalID = dt.Rows[0][0].ToString();
            NotifyOfPropertyChange(() => criminalID);
            _firstname = dt.Rows[0][1].ToString();
            NotifyOfPropertyChange(() => firstname);
            _middlename = dt.Rows[0][2].ToString();
            NotifyOfPropertyChange(() => middlename);
            _lastname = dt.Rows[0][3].ToString();
            NotifyOfPropertyChange(() => lastname);
            _selectedSex = dt.Rows[0][4].ToString();
            NotifyOfPropertyChange(() => selectedSex);
            _birthdateSelectedDate = (DateTime)dt.Rows[0][5];
            NotifyOfPropertyChange(() => birthdateSelectedDate);
            NotifyOfPropertyChange(() => age);
            _birthplace = dt.Rows[0][7].ToString();
            NotifyOfPropertyChange(() => birthplace);
            _address = dt.Rows[0][8].ToString();
            NotifyOfPropertyChange(() => address);
            _crime = dt.Rows[0][9].ToString();
            NotifyOfPropertyChange(() => crime);
            _placeofarrest = dt.Rows[0][10].ToString();
            NotifyOfPropertyChange(() => _placeofarrest);
            _arrestingofficer = dt.Rows[0][11].ToString();
            NotifyOfPropertyChange(() => arrestingofficer);
            _dateofarrestSelectedDate = (DateTime)dt.Rows[0][12];
            NotifyOfPropertyChange(() => dateofarrestSelectedDate);
            _eyecolor = dt.Rows[0][13].ToString();
            NotifyOfPropertyChange(() => eyecolor);
            _haircolor = dt.Rows[0][14].ToString();
            NotifyOfPropertyChange(() => haircolor);
            _remarks = dt.Rows[0][15].ToString();
            NotifyOfPropertyChange(() => remarks);
            if (dt.Rows[0][16].ToString() != string.Empty)
            {
                ImageSource imgsource = new BitmapImage(new Uri(dt.Rows[0][16].ToString()));
                _criminalPictureSource = imgsource;
                NotifyOfPropertyChange(() => criminalPictureSource);
            }
        }

        public void saveButton()
        {
            if (areRequiredFieldsComplete() && _hasPicture)
            {
                savePicture();
                connection.dbCommand("UPDATE `ps4`.`criminalrecords` SET `First_Name` = '" + _firstname + "', `Middle_Name` = '" + _middlename + "', `Last_Name` = '" + _lastname + "', `Sex` = '" + _selectedSex + "', `Birthdate` = '" + _birthdateSelectedDate.ToString("yyyy-MM-dd") + "', `Age` = '" + _age + "', `Birthplace` = '" + _birthplace + "', `Address` = '" + _address + "', `Crime` = '" + _crime + "', `Place_of_Arrest` = '" + _placeofarrest + "', `Arresting_Officer` = '" + _arrestingofficer + "', `Date_of_Arrest` = '" + _dateofarrestSelectedDate.ToString("yyyy-MM-dd") + "', `Eye_Color` = '" + _eyecolor + "', `Hair_Color` = '" + _haircolor + "', `Remarks` = '" + _remarks + "', `Picture` = '" + _savedCriminalPictureFilePath + "' WHERE (`Criminal_ID` = '" + _criminalID + "');");
                TryClose();
            }
            else if (areRequiredFieldsComplete())
            {
                connection.dbCommand("UPDATE `ps4`.`criminalrecords` SET `First_Name` = '" + _firstname + "', `Middle_Name` = '" + _middlename + "', `Last_Name` = '" + _lastname + "', `Sex` = '" + _selectedSex + "', `Birthdate` = '" + _birthdateSelectedDate.ToString("yyyy-MM-dd") + "', `Age` = '" + _age + "', `Birthplace` = '" + _birthplace + "', `Address` = '" + _address + "', `Crime` = '" + _crime + "', `Place_of_Arrest` = '" + _placeofarrest + "', `Arresting_Officer` = '" + _arrestingofficer + "', `Date_of_Arrest` = '" + _dateofarrestSelectedDate.ToString("yyyy-MM-dd") + "', `Eye_Color` = '" + _eyecolor + "', `Hair_Color` = '" + _haircolor + "', `Remarks` = '" + _remarks + "' WHERE (`Criminal_ID` = '" + _criminalID + "');");
                TryClose();
            }
            else
            {
                MessageBox.Show("Please fill out all required fields");
            }
        }

        public void cancelButton()
        {
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure? Unsaved changes will be lost.", "!", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                TryClose();
            }
        }

        public void changePictureButton()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = false;
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _criminalPictureFilePath = op.FileName;
                ImageSource imgsource = new BitmapImage(new Uri(_criminalPictureFilePath));
                _criminalPictureSource = imgsource;
                this.NotifyOfPropertyChange(() => this.criminalPictureSource);
                _hasPicture = true;
            }
        }

        public void takePictureButton()
        {
        }

        private static void savePicture()
        {
            string name = System.IO.Path.GetFileName(_criminalPictureFilePath);
            string destinationPath = getAppStartPath(name, "criminalPictures");
            destinationPath = destinationPath.Replace(@"\", @"\\");
            destinationPath = destinationPath.Replace(@"\\\\", @"\\");
            _savedCriminalPictureFilePath = destinationPath;
            File.Copy(_criminalPictureFilePath, destinationPath, true);
        }

        private static String getAppStartPath(string filename, string foldername)
        {
            String appStartPath = System.AppDomain.CurrentDomain.BaseDirectory;
            appStartPath = String.Format(appStartPath + @"\{0}\" + filename, foldername);
            return appStartPath;
        }

        private bool areRequiredFieldsComplete()
        {
            if (
                _firstname == string.Empty ||
                _middlename == string.Empty ||
                _lastname == string.Empty ||
                _selectedSex == string.Empty ||
                _crime == string.Empty ||
                _placeofarrest == string.Empty ||
                _arrestingofficer == string.Empty ||
                _selectedSex == string.Empty
                )
                return false;
            else
                return true;
        }

        public DateTime birthdateSelectedDate
        {
            get { return _birthdateSelectedDate; }
            set
            {
                _birthdateSelectedDate = value;
                NotifyOfPropertyChange(() => age);
            }
        }

        public string age
        {
            get { return (DateTime.Now.Year - _birthdateSelectedDate.Year).ToString(); }
            set
            {
                _age = value;
                NotifyOfPropertyChange(() => age);
            }
        }

        public DateTime dateofarrestSelectedDate
        {
            get { return _dateofarrestSelectedDate; }
            set { _dateofarrestSelectedDate = value; }
        }

        public string criminalID
        {
            get { return _criminalID; }
            set { _criminalID = value; }
        }

        public string firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        public string middlename
        {
            get { return _middlename; }
            set { _middlename = value; }
        }

        public string lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }

        public List<string> sex
        {
            get { return new List<string> { "Male", "Female" }; }
            set { _sex = value; }
        }

        public ImageSource criminalPictureSource
        {
            get { return this._criminalPictureSource; }
            set { this._criminalPictureSource = value; }
        }

        public string selectedSex
        {
            get { return this._selectedSex; }

            set
            {
                this._selectedSex = value;
                this.NotifyOfPropertyChange(() => this.selectedSex);
            }
        }

        public DateTime birthdate
        {
            get { return _birthdate; }
            set
            {
                _birthdate = value;
                NotifyOfPropertyChange(() => age);
            }
        }

        public string birthplace
        {
            get { return _birthplace; }
            set { _birthplace = value; }
        }

        public string address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string crime
        {
            get { return _crime; }
            set { _crime = value; }
        }

        public string placeofarrest
        {
            get { return _placeofarrest; }
            set { _placeofarrest = value; }
        }

        public string arrestingofficer
        {
            get { return _arrestingofficer; }
            set { _arrestingofficer = value; }
        }

        public DateTime dateofarrest
        {
            get { return _dateofarrest; }
            set { _dateofarrest = value; }
        }

        public string eyecolor
        {
            get { return _eyecolor; }
            set { _eyecolor = value; }
        }

        public string haircolor
        {
            get { return _haircolor; }
            set { _haircolor = value; }
        }

        public string remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        public string criminalpicture
        {
            get { return _criminalPicture; }
            set { _criminalPicture = value; }
        }
    }
}