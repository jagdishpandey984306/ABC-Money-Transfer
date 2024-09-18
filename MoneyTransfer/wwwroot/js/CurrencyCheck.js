
const select = document.querySelectorAll('.currency');
const number = document.getElementById("number");
const output = document.getElementById("output");
const exchangeRate = document.getElementById("Exchange_Rate"); 
const unit = document.getElementById("Unit");
const selectDate = document.getElementById("SelectDate");
const apiUrl = 'https://www.nrb.org.np/api/forex/v1/rates';


$(function () {
    let currentDate = formatDate(new Date());
    loadData(1, 100, currentDate, currentDate);
});

function search() {
    loadData(1, 5, selectDate.value, selectDate.value)
}


async function loadData(page, per_page, fromDate, toDate) {
    var tableBody = document.getElementById("tbody");
    const url = `${apiUrl}?page=${page}&per_page=${per_page}&from=${fromDate}&to=${toDate}`;
    try {
        document.getElementById("error").innerHTML = "";
        const response = await fetch(url);
        if (!response.ok) {
            tableBody.innerHTML = "";
            document.getElementById("error").innerHTML = `<b class="text-danger">HTTP error! Status: ${response.status}</b>`;
        }
        const result = await response.json();
        const payload = result.data.payload;

        var rowsHtml = payload[0].rates.map((item, index) =>
            `<tr>
                <td>${index + 1}</td>
                <td>${item.currency.iso3} (${item.currency.name})</td>
                <td>${item.currency.unit}</td>
                <td>${item.buy}</td>
                <td>${item.sell}</td>
                </tr>`
        )
            .join("");

        tableBody.innerHTML = rowsHtml;
    } catch (error) {
        tableBody.innerHTML = "";
        document.getElementById("error").innerHTML = `<b class="text-danger">Error fetching data:${error}</b>`;
    }
}

fetch(`${apiUrl}?page=${1}&per_page=${5}&from=${formatDate(new Date())}&to=${formatDate(new Date())}`)
    .then((result) => result.json())
    .then((result) => {
        display(result.data.payload[0].rates);
    });


function display(data) {
    data.map((item) => {
        select[0].innerHTML += `<option value="${item.currency.iso3}">${item.currency.iso3} (${item.currency.name})</option>`;
    });
}

function updatevalue() {
    try {
        let currency1 = select[0].value;
        //let currency2 = select[1].value;
        let value = number.value;

        const url = `${apiUrl}?page=${1}&per_page=${100}&from=${formatDate(new Date())}&to=${formatDate(new Date())}`;

        fetch(url)
            .then((result) => result.json())
            .then((result) => {
                const data = result.data.payload[0].rates;

                // Find the rate for the selected currency
                const rateItem = data.find(item => item.currency.iso3 === currency1);

                if (rateItem) {
                    exchangeRate.value = rateItem.sell;
                    unit.value = rateItem.currency.unit;
                    // Convert the value using the found rate
                    convert(rateItem.currency.unit,rateItem.currency.iso3,rateItem.sell, value);
                } else {
                    console.error('Currency rate not found.');
                }
            });
    }
    catch (error) {
        console.error('Error fetching data:', error);
    }
}


function convert(unit, currencyCode, rate, amount) {
    let finalRate = rate / unit;
    const total = finalRate * amount;
    output.value = total.toFixed(2);
    document.getElementById("currencyMessage").innerHTML = `<b>1 ${currencyCode} = ${finalRate.toFixed(2) } NPR </b><br/>
    ${amount > 1 ? `<b>${amount}${currencyCode}=NPR ${total.toFixed(2) }</b>`:``}<br/>
    <button type="submit" class="btn btn-md btn-outline-success mt-3">
    <i class="bi bi-currency-rupee"></i> Send Money
    </button>`;
}


function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}