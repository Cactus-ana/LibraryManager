using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManager
{
    public partial class Form1 : Form
    {
        private Library library;
        private DataGridView grid;
        private TextBox txtTitle, txtAuthor, txtYear, txtSearch;
        private ComboBox cmbGenre, cmbFilterGenre, cmbFilterStatus, cmbSort;
        private Button btnAdd, btnEdit, btnDelete, btnStatus, btnSearch, btnReset;

        public Form1()
        {
            InitializeComponent();
            library = new Library();
            CreateControls();
            LoadData();
        }

        private void CreateControls()
        {
            this.Text = "Библиотечный менеджер";
            this.Size = new Size(1100, 700);

            // ТАБЛИЦА
            grid = new DataGridView
            {
                Location = new Point(12, 12),
                Size = new Size(750, 630),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            this.Controls.Add(grid);

            // ГРУППА ДОБАВЛЕНИЯ
            GroupBox gb1 = new GroupBox
            {
                Text = "Добавление / Редактирование",
                Location = new Point(780, 12),
                Size = new Size(290, 290),
                BackColor = Color.AliceBlue
            };

            gb1.Controls.Add(new Label { Text = "Название:", Location = new Point(10, 25), AutoSize = true });
            txtTitle = new TextBox { Location = new Point(100, 22), Size = new Size(180, 25) };
            gb1.Controls.Add(txtTitle);

            gb1.Controls.Add(new Label { Text = "Автор:", Location = new Point(10, 60), AutoSize = true });
            txtAuthor = new TextBox { Location = new Point(100, 57), Size = new Size(180, 25) };
            gb1.Controls.Add(txtAuthor);

            gb1.Controls.Add(new Label { Text = "Год:", Location = new Point(10, 95), AutoSize = true });
            txtYear = new TextBox { Location = new Point(100, 92), Size = new Size(180, 25) };
            gb1.Controls.Add(txtYear);

            gb1.Controls.Add(new Label { Text = "Жанр:", Location = new Point(10, 130), AutoSize = true });
            cmbGenre = new ComboBox
            {
                Location = new Point(100, 127),
                Size = new Size(180, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbGenre.Items.AddRange(new string[] { "Роман", "Фантастика", "Поэзия", "Детектив", "Научная" });
            gb1.Controls.Add(cmbGenre);

            btnAdd = new Button
            {
                Text = "Добавить",
                Location = new Point(10, 175),
                Size = new Size(130, 35),
                BackColor = Color.LightGreen
            };
            btnAdd.Click += BtnAdd_Click;
            gb1.Controls.Add(btnAdd);

            btnEdit = new Button
            {
                Text = "Изменить",
                Location = new Point(150, 175),
                Size = new Size(130, 35),
                BackColor = Color.LightYellow
            };
            btnEdit.Click += BtnEdit_Click;
            gb1.Controls.Add(btnEdit);

            btnDelete = new Button
            {
                Text = "Удалить",
                Location = new Point(10, 220),
                Size = new Size(130, 35),
                BackColor = Color.LightCoral
            };
            btnDelete.Click += BtnDelete_Click;
            gb1.Controls.Add(btnDelete);

            btnStatus = new Button
            {
                Text = "Выдать/Вернуть",
                Location = new Point(150, 220),
                Size = new Size(130, 35),
                BackColor = Color.LightBlue
            };
            btnStatus.Click += BtnStatus_Click;
            gb1.Controls.Add(btnStatus);

            this.Controls.Add(gb1);

            // ГРУППА ФИЛЬТРОВ
            GroupBox gb2 = new GroupBox
            {
                Text = "Поиск и фильтры",
                Location = new Point(780, 315),
                Size = new Size(290, 280),
                BackColor = Color.MistyRose
            };

            gb2.Controls.Add(new Label { Text = "Поиск:", Location = new Point(10, 25), AutoSize = true });
            txtSearch = new TextBox { Location = new Point(70, 22), Size = new Size(140, 25) };
            gb2.Controls.Add(txtSearch);

            btnSearch = new Button { Text = "Найти", Location = new Point(220, 22), Size = new Size(60, 28) };
            btnSearch.Click += (s, e) => LoadData();
            gb2.Controls.Add(btnSearch);

            gb2.Controls.Add(new Label { Text = "Жанр:", Location = new Point(10, 65), AutoSize = true });
            cmbFilterGenre = new ComboBox
            {
                Location = new Point(70, 62),
                Size = new Size(210, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilterGenre.SelectedIndexChanged += (s, e) => LoadData();
            gb2.Controls.Add(cmbFilterGenre);

            gb2.Controls.Add(new Label { Text = "Статус:", Location = new Point(10, 105), AutoSize = true });
            cmbFilterStatus = new ComboBox
            {
                Location = new Point(70, 102),
                Size = new Size(210, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilterStatus.Items.AddRange(new string[] { "Все", "Available", "Issued" });
            cmbFilterStatus.SelectedIndex = 0;
            cmbFilterStatus.SelectedIndexChanged += (s, e) => LoadData();
            gb2.Controls.Add(cmbFilterStatus);

            gb2.Controls.Add(new Label { Text = "Сорт.:", Location = new Point(10, 145), AutoSize = true });
            cmbSort = new ComboBox
            {
                Location = new Point(70, 142),
                Size = new Size(210, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSort.Items.AddRange(new string[] { "Нет", "Название", "Автор", "Год" });
            cmbSort.SelectedIndex = 0;
            cmbSort.SelectedIndexChanged += (s, e) => LoadData();
            gb2.Controls.Add(cmbSort);

            btnReset = new Button
            {
                Text = "Сбросить",
                Location = new Point(10, 190),
                Size = new Size(270, 35),
                BackColor = Color.LightSteelBlue
            };
            btnReset.Click += (s, e) =>
            {
                txtSearch.Clear();
                cmbFilterGenre.SelectedIndex = -1;
                cmbFilterStatus.SelectedIndex = 0;
                cmbSort.SelectedIndex = 0;
                LoadData();
            };
            gb2.Controls.Add(btnReset);

            this.Controls.Add(gb2);
        }

        private void LoadData()
        {
            List<Book> books = library.GetAllBooks();

            // Поиск
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                books = library.Search(txtSearch.Text.Trim());

            // Фильтр по жанру
            if (cmbFilterGenre.SelectedItem != null && cmbFilterGenre.SelectedItem.ToString() != "Все жанры")
                books = library.FilterByGenre(cmbFilterGenre.SelectedItem.ToString());

            // Фильтр по статусу
            if (cmbFilterStatus.SelectedItem != null && cmbFilterStatus.SelectedItem.ToString() != "Все")
                books = library.FilterByStatus(cmbFilterStatus.SelectedItem.ToString());

            // Сортировка
            if (cmbSort.SelectedItem != null)
            {
                string sort = cmbSort.SelectedItem.ToString();
                if (sort == "Название") books = library.SortBooks("Название");
                else if (sort == "Автор") books = library.SortBooks("Автор");
                else if (sort == "Год") books = library.SortBooks("Год");
            }

            grid.DataSource = books;

            // Обновить жанры в фильтре
            List<string> genres = library.GetAllGenres();
            genres.Insert(0, "Все жанры");
            cmbFilterGenre.Items.Clear();
            cmbFilterGenre.Items.AddRange(genres.ToArray());
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                !int.TryParse(txtYear.Text, out int year) ||
                cmbGenre.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка");
                return;
            }

            library.AddBook(txtTitle.Text, txtAuthor.Text, year, cmbGenre.SelectedItem.ToString());
            ClearFields();
            LoadData();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                !int.TryParse(txtYear.Text, out int year) ||
                cmbGenre.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells[0].Value);
            library.UpdateBook(id, txtTitle.Text, txtAuthor.Text, year, cmbGenre.SelectedItem.ToString());
            ClearFields();
            LoadData();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells[0].Value);
            library.DeleteBook(id);
            ClearFields();
            LoadData();
        }

        private void BtnStatus_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells[0].Value);
            library.ChangeStatus(id);
            LoadData();
        }

        private void ClearFields()
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtYear.Clear();
            cmbGenre.SelectedIndex = -1;
        }
    }
}