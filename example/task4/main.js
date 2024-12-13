const canvas = document.getElementById('webglCanvas');
const gl = canvas.getContext('webgl');
if (!gl) {
    alert('WebGL не поддерживается в вашем браузере.');
}


// Улучшение качества изображения canvas
function resizeCanvasToDisplaySize(canvas) {
    const displayWidth = canvas.clientWidth;
    const displayHeight = canvas.clientHeight;
    const needResize =
        canvas.width !== displayWidth || canvas.height !== displayHeight;

    if (needResize) {
        canvas.width = displayWidth;
        canvas.height = displayHeight;
    }
    return needResize;
}
resizeCanvasToDisplaySize(canvas);
//canvas.width = window.innerWidth;
//canvas.height = window.innerHeight;
gl.viewport(0, 0, gl.canvas.width, gl.canvas.height);

// Шейдеры
const vertexShaderSource = `
    attribute vec4 a_position;
    attribute vec4 a_color;
    
    uniform mat4 u_model_view_matrix;
    
    varying lowp vec4 v_color;
    
    void main(void) {
        gl_Position = u_model_view_matrix * a_position;
        v_color = a_color;
    }
`;

const fragmentShaderSource = `
    precision mediump float;
  varying vec4 v_color;
  
  void main() {
    gl_FragColor = v_color;
  }
`;

// Компиляция шейдера
function compileShader(source, type) {
    const shader = gl.createShader(type);
    gl.shaderSource(shader, source);
    gl.compileShader(shader);
    if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
        console.error('Ошибка компиляции шейдера:', gl.getShaderInfoLog(shader));
    }
    return shader;
}

// Создание программы
function createProgram(vertexShader, fragmentShader) {
    const program = gl.createProgram();
    gl.attachShader(program, vertexShader);
    gl.attachShader(program, fragmentShader);
    gl.linkProgram(program);
    if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
        console.error('Ошибка линковки программы:', gl.getProgramInfoLog(program));
    }
    return program;
}

// Компиляция шейдеров и создание программы
const vertexShader = compileShader(vertexShaderSource, gl.VERTEX_SHADER);
const fragmentShader = compileShader(fragmentShaderSource, gl.FRAGMENT_SHADER);
const program = createProgram(vertexShader, fragmentShader);
gl.useProgram(program);

// Функция генерации вершин круга
const getCirclePoints = ({
    N = 360,
    xCenter = 0,
    yCenter = 0,
    radius = 0.5,
} = {}) => {
    const points = [xCenter, yCenter, 0, 1.0];

    for (let i = 0; i < N + 1; i++) {
        const angle = (i * (2.0 * Math.PI)) / N;
        const x = xCenter + radius * Math.cos(angle);
        const y = yCenter + radius * Math.sin(angle);

        points.push(x);
        points.push(y);
        points.push(0);
        points.push(1.0);
    }

    return points;
};

// Функция преобразования HSV в RGB
const HSVtoRGB = (h, s, v) => {
    let r, g, b;
    
    const i = Math.floor(h * 6);
    const f = h * 6 - i;
    const p = v * (1 - s);
    const q = v * (1 - f * s);
    const t = v * (1 - (1 - f) * s);
    
    switch (i % 6) {
        case 0: r = v; g = t; b = p; break;
        case 1: r = q; g = v; b = p; break;
        case 2: r = p; g = v; b = t; break;
        case 3: r = p; g = q; b = v; break;
        case 4: r = t; g = p; b = v; break;
        case 5: r = v; g = p; b = q; break;
    }
    return [r, g, b];
};

// Функция генерации цветов для круга (градирентный цвет от центра к краям)
const getCircleColors = () => {
    let generatedColors = [1.0, 1.0, 1.0, 1.0]; // Цвет центра (белый)

    for (let i = 0; i < 361; i++) {
        const hue = (i / 360) % 1;
        const [r, g, b] = HSVtoRGB(hue, 1, 1);
        generatedColors.push(r, g, b, 1.0); // Добавляем цвета с альфа-каналом
    }

    return generatedColors;
};

// Генерация вершин и цветов
const vertices = new Float32Array(getCirclePoints({ N: 360, radius: 0.5 }));
console.log(vertices);
const colors = new Float32Array(getCircleColors());
console.log(colors);

// Буферы для вершин и цветов
const vbo = gl.createBuffer();
const cbo = gl.createBuffer();

// Привязка и загрузка данных
gl.bindBuffer(gl.ARRAY_BUFFER, vbo);
gl.bufferData(gl.ARRAY_BUFFER, vertices, gl.STATIC_DRAW);

gl.bindBuffer(gl.ARRAY_BUFFER, cbo);
gl.bufferData(gl.ARRAY_BUFFER, colors, gl.STATIC_DRAW);

// Атрибуты вершин
const positionAttribLocation = gl.getAttribLocation(program, 'a_position');
gl.vertexAttribPointer(positionAttribLocation, 4, gl.FLOAT, false, 0, 0);
gl.enableVertexAttribArray(positionAttribLocation);

// Атрибуты цветов
const colorAttribLocation = gl.getAttribLocation(program, 'a_color');
gl.vertexAttribPointer(colorAttribLocation, 4, gl.FLOAT, false, 0, 0);
gl.enableVertexAttribArray(colorAttribLocation);

// Матрица преобразования
const uModelViewMatrix = gl.getUniformLocation(program, 'u_model_view_matrix');

// Инициализация параметров
let scaleX = 1.0;
let scaleY = 1.0;

document.addEventListener('keydown', (event) => {
    switch (event.key) {
        case 'ArrowUp': scaleY += 0.1; break; // Увеличиваем масштаб по оси Y
        case 'ArrowDown': scaleY = Math.max(0.1, scaleY - 0.1); break; // Уменьшаем масштаб по оси Y
        case 'ArrowLeft': scaleX -= 0.1; break; // Уменьшаем масштаб по оси X
        case 'ArrowRight': scaleX += 0.1; break; // Увеличиваем масштаб по оси X
    }
});

function render() {
    gl.clear(gl.COLOR_BUFFER_BIT);

    const modelViewMatrix = mat4.create();
    mat4.scale(modelViewMatrix, modelViewMatrix, [scaleX, scaleY, 1]);

    gl.uniformMatrix4fv(uModelViewMatrix, false, modelViewMatrix);

    gl.drawArrays(gl.TRIANGLE_FAN, 0, vertices.length/4); // Рисуем круг с помощью TRIANGLE_FAN

    requestAnimationFrame(render);
}

gl.clearColor(0, 0, 0, 1); // Чёрный фон
render();
