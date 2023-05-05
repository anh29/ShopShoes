const {cnn, sql} = require('../../connect'); 
module.exports = function() {
    this.create = async function(newData, result) {
        let pool = await cnn;
        let sqlString = "Insert into detailBill(ID_bill, ID_product, Quantity) values (@ID_bill, @ID_product, @Quantity)";
        return await pool.request()
        .input('ID_bill', sql.Int, newData.ID_bill)
        .input('ID_product', sql.Int, newData.ID_product)
        .input('Quantity', sql.Int, newData.Quantity)
        .query(sqlString, function(err, data) {
            if(err == false) {
                result("errol", null);
            }else {
                result(null, newData);
            }
        });
    };
}