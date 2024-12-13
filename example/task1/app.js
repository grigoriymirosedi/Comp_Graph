// Инициализация WebGL
const canvas = document.getElementById('webglCanvas');
const gl = canvas.getContext('webgl');
if (!gl) {
    alert('WebGL не поддерживается в вашем браузере.');
}

//canvas.width = window.innerWidth;
//canvas.height = window.innerHeight;

// Вершинный шейдер для градиентного тетраэдра
const vertexShaderSource = `
  attribute vec4 a_position;
  attribute vec4 a_color;
  uniform mat4 u_modelViewMatrix;
  varying vec4 v_color;
  
  void main() {
    gl_Position = u_modelViewMatrix * a_position;
    v_color = a_color;
  }
`;

// Фрагментный шейдер для отображения цветов вершин
const fragmentShaderSource = `
  precision mediump float;
  varying vec4 v_color;
  
  void main() {
    gl_FragColor = v_color;
  }
`;

// Функция компиляции шейдера
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

// Вершины тетраэдра (позиции и цвета)
const vertices = new Float32Array([
    // x, y, z, r, g, b
    // Грань 1 (ABC)
    0.0, 0.3, 0.0, 1.0, 1.0, 0.0, // A
    - 0.3, -0.3, -0.3, 0.0, 1.0, 0.0, // B
    0.3, -0.3, -0.3, 0.0, 0.0, 1.0, // C

    // Грань 2 (ACD)
    0.0, 0.3, 0.0, 1.0, 1.0, 0.0,  // A
    0.3, -0.3, -0.3, 0.0, 0.0, 1.0,  // C
    0.0, -0.3, 0.3, 1.0, 0.0, 0.0,  // D

    // Грань 3 (ADB)
    0.0, 0.3, 0.0, 1.0, 1.0, 0.0,  // A
    0.0, -0.3, 0.3, 1.0, 0.0, 0.0,  // D
    -0.3, -0.3, -0.3, 0.0, 1.0, 0.0, // B

    // Грань 4 (BDC)
    -0.3, -0.3, -0.3, 0.0, 1.0, 0.0, // B
    0.0, -0.3, 0.3, 1.0, 0.0, 0.0,  // D
    0.3, -0.3, -0.3, 0.0, 0.0, 1.0,  // C
]);

// Создание буфера для вершин
const positionAttribLocation = gl.getAttribLocation(program, 'a_position');
const colorAttribLocation = gl.getAttribLocation(program, 'a_color');
const vbo = gl.createBuffer();
gl.bindBuffer(gl.ARRAY_BUFFER, vbo);
gl.bufferData(gl.ARRAY_BUFFER, vertices, gl.STATIC_DRAW);

gl.vertexAttribPointer(positionAttribLocation, 3, gl.FLOAT, false, 6 * Float32Array.BYTES_PER_ELEMENT, 0);
gl.vertexAttribPointer(colorAttribLocation, 3, gl.FLOAT, false, 6 * Float32Array.BYTES_PER_ELEMENT, 3 * Float32Array.BYTES_PER_ELEMENT);

gl.enableVertexAttribArray(positionAttribLocation);
gl.enableVertexAttribArray(colorAttribLocation);

// Объект для хранения смещения по осям
let translation = { x: 0, y: 0, z: 0 };
let rotation = { x: 0, y: 0, z: 0 }; // Добавим вращение

// Получение uniform-переменной для модели
const modelViewMatrixUniform = gl.getUniformLocation(program, "u_modelViewMatrix");

// Обработчик событий клавиатуры для изменения позиции
document.addEventListener('keydown', (event) => {
    let key_low = event.key.toLocaleLowerCase;

    switch (event.key) {
        case 'y':
            translation.y += 0.1;
            break;
        case 'Y':
            translation.y -= 0.1;
            break;
        case 'x':
            translation.x += 0.1;
            break;
        case 'X':
            translation.x -= 0.1;
            break;
        case 'z':
            translation.z += 0.1;
            break;
        case 'Z':
            translation.z -= 0.1;
            break;    
        case 'w': // поворот
            rotation.x += 5;
            break;
        case 's': // поворот
            rotation.x -= 5;
            break;
        case 'a': // Поворот влево
            rotation.y += 5;
            break;
        case 'd': // Поворот вправо
            rotation.y -= 5;
            break;
    }
    let mat41 = window.mat4;
    // Обновляем модельную матрицу с трансляцией и вращением
    const modelViewMatrix = mat41.create();
    mat4.translate(modelViewMatrix, modelViewMatrix, [translation.x, translation.y, translation.z]);
    mat4.rotate(modelViewMatrix, modelViewMatrix, rotation.x * Math.PI / 180, [1, 0, 0]);
    mat4.rotate(modelViewMatrix, modelViewMatrix, rotation.y * Math.PI / 180, [0, 1, 0]);
    mat4.rotate(modelViewMatrix, modelViewMatrix, rotation.z * Math.PI / 180, [0, 0, 1]);
    gl.uniformMatrix4fv(modelViewMatrixUniform, false, modelViewMatrix);
});

// Инициализация рендеринга
function render() {
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
    gl.drawArrays(gl.TRIANGLES, 0, 12); // Рисуем тетраэдр (3 треугольника)
    requestAnimationFrame(render);
}

// Камера
const projectionMatrix = mat4.create();
mat4.perspective(projectionMatrix, Math.PI / 4, canvas.width / canvas.height, 0.1, 100);
gl.uniformMatrix4fv(modelViewMatrixUniform, false, projectionMatrix);

gl.clearColor(0.0, 0.0, 0.0, 1.0);
gl.enable(gl.DEPTH_TEST);
render();
