const {cnn, sql} = require('../../connect'); 
module.exports = function() {
    this.create = async function(newData, result) {
        let pool = await cnn;
        let sqlString = `Insert into tb_Order(TypePayment, CreateDay, TotalPrice, Discount, ExtraMoney, Total, Note, Phone, Address, Email, CustomerName, UserId)
                         output inserted.Id values (@paymentState, @createDay, @productsMoney, @Discount, @extraMoney, @Total, @Note, @Phone, @Address, @Email, @CustomerName, @UserId)`;
        return await pool.request()
        .input('paymentState', sql.Bit, newData.paymentState)
        .input('createDay', sql.DateTime, newData.createDay)
        .input('productsMoney', sql.Decimal, newData.productsMoney)
        .input('Discount', sql.Decimal, newData.Discount)
        .input('extraMoney', sql.Decimal, newData.extraMoney)
        .input('Total', sql.Decimal, newData.Total)
        .input('Note', sql.NVarChar, newData.Note)
        .input('Phone', sql.Float, newData.Phone)
        .input('Address', sql.NVarChar, newData.Address)
        .input('Email', sql.NVarChar, newData.Email)
        .input('CustomerName', sql.NVarChar, newData.CustomerName)
        .input('UserId', sql.NVarChar, newData.UserId)
        .query(sqlString, function(err, data) {
            if(err == false) {
                result("errol", null);
            }else {
                result(null, data.recordset[0].Id);
            }
        });
    };
}