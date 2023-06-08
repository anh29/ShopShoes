// let http = require('http'); //global
// let server = http.createServer(function(req, res) {
//     res.end('<h1>Hello nhi lun</h1>');
// });// 
//cac route
// app.get('', function(req, res) {
//     res.send('<form method="POST" action="/register"><button>Register</button></form>');
// });
// server.listen(3000, function() {
//     console.log('Dang mo may chu tai: http://localhost:3000')
// });


let express = require('express');
let cors = require('cors');
let morgan = require('morgan');
let bodyParser = require("body-parser");
let app = express();
app.use(morgan('dev'));
app.use(cors());
app.use(bodyParser.json());

var productRoute = require('./app/routes/product.route');
var customerRoute = require('./app/routes/customer.route');
var clientRoute = require('./app/routes/client.route');
var billRoute = require('./app/routes/bill.route');
var detailBillRoute = require('./app/routes/detailBill.route');
productRoute(app);
customerRoute(app);
clientRoute(app);
billRoute(app);
detailBillRoute(app);

app.listen(4000, function() {
    console.log("http://localhost:4000")
});

// let express = require('express');
// let cors = require('cors');
// let app = express();
// app.use(cors());

// app.get("/getData", (req, res) => {
//     res.send("hello");
// });

// app.listen(5000, function() {
//     console.log("http://localhost:5000")
// });