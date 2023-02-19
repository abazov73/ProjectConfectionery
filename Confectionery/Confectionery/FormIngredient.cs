using ConfectioneryContracts.BindingModels;
using ConfectioneryContracts.BusinessLogicsContracts;
using ConfectioneryContracts.SearchModels;
using Microsoft.Extensions.Logging;
namespace Confectionery
{
    public partial class FormIngredient : Form
    {
        private readonly ILogger  _logger;
        private readonly IIngredientLogic _logic;
        private int? _id;
        public int Id { set { _id = value; } }

        public FormIngredient(ILogger<FormIngredient> logger, IIngredientLogic logic)
        {
            InitializeComponent();
            _logger = logger;
            _logic = logic;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("��������� ��������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _logger.LogInformation("���������� �����������");
            try
            {
                var model = new IngredientBindingModel
                {
                    Id = _id ?? 0,
                    IngredientName = textBoxName.Text,
                    Cost = Convert.ToDouble(textBoxCost.Text)
                };
                var operationResult = _id.HasValue ? _logic.Update(model) : _logic.Create(model);
                if (!operationResult)
                {
                    throw new Exception("������ ��� ����������. �������������� ���������� � �����.");
                }
                MessageBox.Show("���������� ������ �������", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ���������� ����������");
                MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormIngredient_Load(object sender, EventArgs e)
        {
            if (_id.HasValue)
            {
                try
                {
                    _logger.LogInformation("��������� ����������");
                    var view = _logic.ReadElement(new IngredientSearchModel { Id = _id.Value });
                    if (view != null)
                    {
                        textBoxName.Text = view.IngredientName;
                        textBoxCost.Text = view.Cost.ToString();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "������ ��������� ����������");
                    MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}