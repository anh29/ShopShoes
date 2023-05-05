const {cnn, sql} = require('../../connect'); 
module.exports = function() {
    this.create = async function(newData, result) {
        let pool = await cnn;
        let sqlString = "Insert into Bill(billState, paymentState, ID_User, ID_Customer, createDay, productsMoney, Discount, extraMoney, Total, Note) output inserted.ID_Bill values (@billState, @paymentState, @ID_User, @ID_Customer, @createDay, @productsMoney, @Discount, @extraMoney, @Total, @Note)";
        return await pool.request()
        .input('billState', sql.NVarChar, newData.billState)
        .input('paymentState', sql.Bit, newData.paymentState)
        .input('ID_User', sql.Int, newData.ID_User)
        .input('ID_Customer', sql.Int, newData.ID_Customer)
        .input('createDay', sql.DateTime, newData.createDay)
        .input('productsMoney', sql.Decimal, newData.productsMoney)
        .input('Discount', sql.Decimal, newData.Discount)
        .input('extraMoney', sql.Decimal, newData.extraMoney)
        .input('Total', sql.Decimal, newData.Total)
        .input('Note', sql.NVarChar, newData.Note)
        .query(sqlString, function(err, data) {
            if(err == false) {
                result("errol", null);
            }else {
                result(null, data.recordset[0].ID_Bill);
            }
        });
    };
}