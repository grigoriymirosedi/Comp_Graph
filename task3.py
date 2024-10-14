import cv2
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.widgets import Slider

# Функция для преобразования RGB в HSV
def rgb_to_hsv(r, g, b):
    r, g, b = r / 255.0, g / 255.0, b / 255.0
    max_c = max(r, g, b)
    min_c = min(r, g, b)
    delta = max_c - min_c

    # Определение яркости
    v = max_c

    # Определение насыщенности
    s = 0 if max_c == 0 else delta / max_c

    # Определение оттенка
    if delta == 0:
        h = 0
    elif max_c == r:
        h = 60 * (((g - b) / delta) % 6)
    elif max_c == g:
        h = 60 * (((b - r) / delta) + 2)
    elif max_c == b:
        h = 60 * (((r - g) / delta) + 4)

    return h, s, v

# Функция для преобразования HSV в RGB
def hsv_to_rgb(h, s, v):
    c = v * s
    x = c * (1 - abs((h / 60) % 2 - 1))
    m = v - c

    if 0 <= h < 60:
        r, g, b = c, x, 0
    elif 60 <= h < 120:
        r, g, b = x, c, 0
    elif 120 <= h < 180:
        r, g, b = 0, c, x
    elif 180 <= h < 240:
        r, g, b = 0, x, c
    elif 240 <= h < 300:
        r, g, b = x, 0, c
    elif 300 <= h < 360:
        r, g, b = c, 0, x

    r, g, b = (r + m) * 255, (g + m) * 255, (b + m) * 255
    return int(r), int(g), int(b)

# Функция для обновления изображения с новыми параметрами HSV
def update(val):
    h_shift = h_slider.val
    s_scale = s_slider.val / 50
    v_scale = v_slider.val / 50

    # Создание копии изображения
    modified_image = image.copy()

    # Преобразование каждого пикселя из RGB в HSV
    for i in range(modified_image.shape[0]):
        for j in range(modified_image.shape[1]):
            r, g, b = modified_image[i, j]
            h, s, v = rgb_to_hsv(r, g, b)

            # Применение сдвигов и масштабирования
            h = (h + h_shift) % 360
            s = np.clip(s * s_scale, 0, 1)
            v = np.clip(v * v_scale, 0, 1)

            # Преобразование обратно в RGB
            r, g, b = hsv_to_rgb(h, s, v)
            modified_image[i, j] = [r, g, b]

    # Обновление изображения на графике
    ax_mod.imshow(modified_image)
    fig.canvas.draw_idle()

# Загрузка изображения
image_path = 'FRUITS.jpg'
image = cv2.cvtColor(cv2.imread(image_path), cv2.COLOR_BGR2RGB)

# Настройка окна графиков
fig, (ax_orig, ax_mod) = plt.subplots(1, 2, figsize=(10, 6))
plt.subplots_adjust(left=0.25, bottom=0.35)

# Отображение оригинального изображения
ax_orig.imshow(image)
ax_orig.set_title('Original Image')
ax_orig.axis('off')

# Отображение модифицированного изображения
ax_mod.imshow(image)
ax_mod.set_title('Modified Image')
ax_mod.axis('off')

# Создание ползунков для изменения Hue, Saturation и Value
ax_h = plt.axes([0.25, 0.25, 0.65, 0.03], facecolor='lightgray')
ax_s = plt.axes([0.25, 0.18, 0.65, 0.03], facecolor='lightgray')
ax_v = plt.axes([0.25, 0.11, 0.65, 0.03], facecolor='lightgray')

h_slider = Slider(ax_h, 'Hue', 0, 360, valinit=0)
s_slider = Slider(ax_s, 'Saturation', 0, 150, valinit=50)
v_slider = Slider(ax_v, 'Value', 0, 150, valinit=50)

# Обновление изображения при изменении ползунков
h_slider.on_changed(update)
s_slider.on_changed(update)
v_slider.on_changed(update)

# Кнопка для сохранения изображения с текущими настройками HSV
save_button = plt.axes([0.25, 0.02, 0.15, 0.04])
button = plt.Button(save_button, 'Save Image')

# Функция сохранения изображения
def save_image(event):
    h_shift = h_slider.val
    s_scale = s_slider.val / 50
    v_scale = v_slider.val / 50

    modified_image = image.copy()

    # Преобразование каждого пикселя из RGB в HSV и обратно
    for i in range(modified_image.shape[0]):
        for j in range(modified_image.shape[1]):
            r, g, b = modified_image[i, j]
            h, s, v = rgb_to_hsv(r, g, b)

            h = (h + h_shift) % 360
            s = np.clip(s * s_scale, 0, 1)
            v = np.clip(v * v_scale, 0, 1)

            r, g, b = hsv_to_rgb(h, s, v)
            modified_image[i, j] = [r, g, b]

    # Сохранение изображения
    cv2.imwrite('modified_image.jpg', cv2.cvtColor(modified_image, cv2.COLOR_RGB2BGR))
    print('Изображение сохранено как "modified_image.jpg"')

# Привязка функции сохранения к кнопке
button.on_clicked(save_image)

# Отображение графика с ползунками
plt.show()
