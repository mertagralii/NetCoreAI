﻿namespace NetCoreAI.Project02.ApiConsumeUI.Dtos
{
    public class GetByIdCustomerDto
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public decimal Balance { get; set; }
    }
}
