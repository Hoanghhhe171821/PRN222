namespace AssignmentPRN222.Models.Vnpay
{
    public class PaymentInformationModel
    {
        public string OrderType { get; set; }

        public string Code { get; set; } = ""; // mã giảm giá
        public string SelectedSeats { get; set; } = ""; // ghế chọn
        public int ShowTimeId { get; set; } // showtime

        public int Price { get; set; } //Thành tiền
        public string PaymentMethod { get; set; } // PHương thức thanh toán

    }
}
