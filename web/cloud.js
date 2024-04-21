
const width = 300;
const height = 300;
const numClouds = 5;

cloudData = () => {
    let data = [];
    for (let i=0; i<numClouds; i++) {
      data.push({
        color:cloudColors(random())
      })
    }
    return data;
}

cloudColors = () => {
    return d3.scaleLinear().domain([0, 1])
      .range([
          "#ffffff",
          "#bfbfbf"
      ]);
  }

  
seed = 1;
random = d3.randomLcg(seed);

function renderClouds()
{
    const svg = generateClouds();
    document.body.append(svg);
    //console.log(svg);
}

function generateClouds() {
    const svg = d3.create("svg")
        .attr("viewBox", [0, 0, width, height])
        .attr("width", "300px")
        .attr("height", "300px")
    
    svg.append("rect")
      .attr("width", width)
      .attr("height", height)
      //.attr("fill", "#292929");
      .attr("fill",'none')
    
    const allClouds = svg.selectAll("ellipse").data(cloudData);
    
    allClouds.enter().append("ellipse")
    .attr("cx", function(d) {
        return randomIntFromInterval(width*.3, height*.7);
      //return (Math.floor(random() * width));
    })
    .attr("cy", function(d,i) {
        return randomIntFromInterval(width*.3, height*.7);
    })
    .attr("rx", "100")
    .attr("ry", "10")
    .attr("class", "cloud")
    .style("fill", "#ffffff")//d => d.color);
    
    return svg.node();
}

function saveSvg(svgEl, name) {
    svgEl.setAttribute("xmlns", "http://www.w3.org/2000/svg");
    var svgData = svgEl.outerHTML;
    var preface = '<?xml version="1.0" standalone="no"?>\r\n';
    var svgBlob = new Blob([preface, svgData], {type:"image/svg+xml;charset=utf-8"});
    var svgUrl = URL.createObjectURL(svgBlob);
    var downloadLink = document.createElement("a");
    downloadLink.href = svgUrl;
    downloadLink.download = name;
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}


function randomIntFromInterval(min, max) { // min and max included 
  return Math.floor(Math.random() * (max - min + 1) + min)
}

document.getElementById('generateBtn').addEventListener('click',  () => { renderClouds()} ) ;

