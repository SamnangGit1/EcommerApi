﻿namespace Eletronic_Api.Model
{
    public class OtpStore
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Otp { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
