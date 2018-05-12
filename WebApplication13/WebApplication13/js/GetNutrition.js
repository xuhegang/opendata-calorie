$(document).ready(function () {
    //htmlobj = $.ajax({ url: "/Home/GetArray", alync: false }).responseText;
    
    //alert(htmlobj);

    $.get("/Home/GetArray", function (result) {
        var p = result.split(",");

        var data = [
            {
                "nutrition": "Total Fat",
                "percentage": +p[0],

            },
            {
                "nutrition": "Cholesterol",
                "percentage": +p[1],
            },
            {
                "nutrition": "Sodium",
                "percentage": +p[2],

            },
            {
                "nutrition": "Potassium",
                "percentage": +p[3],
            },
            {
                "nutrition": "Total Carbohydrates",
                "percentage": +p[4],

            },
            {
                "nutrition": "Protein",
                "percentage": +p[5],
            }
        ];
        console.log(data);
        var margin = { top: 50, right: 50, bottom: 50, left: 50 };

        var width = 800;//window.screen.width    - margin.left - margin.right;
        var height = 400;


        //////////////////////////////////////////////////////////////////////////////


        var svg = d3.select("#chart")
            .append('svg')

            //The total width of our svg is the width of the graph, plus the size of the left and right margins
            .attr('width', width + margin.left + margin.right)

            //The total height of our svg is the height of the graph, plus the size of the top and bottom margins
            .attr('height', height + margin.top + margin.bottom)

            //We can't see the svg yet - but if you want to check it's worked, you can set the background colour to pink!
            //.style('background-color', 'pink')

            //Then, we're going to add a "group", so we can move the whole graph in one go
            //This lets us make room for the margins easily
            .append('g')
            .attr('transform', 'translate(' + margin.left + ',' + margin.top + ")");


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //data = data.sort(function(a, b){ return b.birthrate - a.birthrate; })

        //A Band scale is good for discrete values, such as the number of bars in a bar chart
        var x = d3.scaleBand()
            .domain(data.map(function (d) { return d.nutrition; }))
            .range([0, width])

            .padding(0.1);


        //This tells us the width of each bar that will fit nicely in our chart,
        //given the size of our graph, the number of inputs, and the amount of padding
        //console.log(x.bandwidth());

        //A Linear scale is good for continuous values, such as real numbers
        var y = d3.scaleLinear()
            .domain([0, d3.max(data, function (d) { return d.percentage; })])
            .range([height, 0]);



        //This tells us how many pixels tall our bar would be, to represent an input of "8"
        //console.log(y(8));


        ///////////////////////////////////////////

        svg.selectAll(".bar")
            .data(data)
            .enter()
            .append("rect")
            .attr("class", "bar")
            .attr("x", function (d) { return x(d.nutrition); })
            .attr("y", function (d) { return y(d.percentage); })
            .attr("width", x.bandwidth())
            .attr("height", function (d) { return height - y(d.percentage); })
            .attr("fill", "orange");

        /*
                            svg.selectAll('text').data(data).enter() //補上資料數值
                  .append('text') 
                  .text(function(d){ return d.percentage}) //將值寫到SVG上
                  .attr({
                    'x': function(d, i){return xScale(i) + barWidth/2},
                    //和上面相同，算出X、Y的位置
                    'y': function(d){return h - yScale(d.percentage) + 15}, //數值放在bar 內
                    'fill': 'white', //文字填滿為白色
                    'text-anchor': 'middle', //文字置中
                    'font-size': '10px' //Fill、font-size也可以用CSS寫喔～
                  });*/



        /////////////////////////////////////////////////////////////////////


        // add the x Axis
        svg.append("g")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x))
            .append("text")
            .attr("fill", "#FFFFFF")
            .attr("y", 40)
            .attr("x", width / 2)
            .attr("text-anchor", "middle")
            .attr("class", "axisRed")
            .text("Ｎutritions");

        // add the y Axis
        svg.append("g")
            .call(d3.axisLeft(y))
            .append("text")
            .attr("fill", "white")
            .attr("y", -40)
            .attr("x", -height / 2)
            .attr("transform", "rotate(-90)")
            .attr("text-anchor", "middle")
            .attr("class", "axisRed")
            .text("Daily Ｎutrition（％）");


                /////////////////////////////////////////////////////////////////////


               // var color = d3.scaleOrdinal(d3.schemeCategory10);
                //svg.selectAll("rect")
                //.attr("fill", function(d, i){ return color(d.nutrition); });
    });
});

