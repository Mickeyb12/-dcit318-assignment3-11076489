using System;
using System.Collections.Generic;

namespace FinanceManagement
{
    public record Transaction(int ID, DateTime Date, decimal Amount, string Category);

    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Bank transfer of {transaction.Amount:C} for '{transaction.Category}'");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Mobile money transfer of {transaction.Amount:C} for '{transaction.Category}'");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Crypto wallet transfer of {transaction.Amount:C} for '{transaction.Category}'");
        }
    }

    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

     public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New balance: {Balance}");
        }

        public sealed class SavingsAccount : Account
        {
            public SavingsAccount(string accountNumber, decimal initialBalance)
                : base(accountNumber, initialBalance) { }

            public override void ApplyTransaction(Transaction transaction)
            {
                if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds.");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New balance: {Balance}");
        }
            }
        }

        public class FinanceApp
        {
            private readonly List<Transaction> _transactions = new();

            public void Run()
            {
                var account = new SavingsAccount("SA-001", 1000m);

                var t1 = new Transaction(1, DateTime.Now, 120.50m, "Groceries");
                var t2 = new Transaction(2, DateTime.Now, 200m, "Utilities");
                var t3 = new Transaction(3, DateTime.Now, 150m, "Entertainment");

                ITransactionProcessor momo = new MobileMoneyProcessor();
                ITransactionProcessor bank = new BankTransferProcessor();
                ITransactionProcessor crypto = new CryptoWalletProcessor();

                momo.Process(t1);
                bank.Process(t2);
                crypto.Process(t3);

                account.ApplyTransaction(t1);
                account.ApplyTransaction(t2);
                account.ApplyTransaction(t3);

                _transactions.AddRange(new[] { t1, t2, t3 });

                Console.WriteLine("Transaction history:");
                foreach (var transaction in _transactions)
                {
                    Console.WriteLine($"Transaction ID: {transaction.ID}, Date: {transaction.Date}, Amount: {transaction.Amount}, Category: {transaction.Category}");
                }
            }

    }
        }
}
