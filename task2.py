import numpy as np
from PIL import Image
import matplotlib.pyplot as plt

# Функция для выделения отдельных каналов R, G, B
def extract_rgb_channels(image):
    image_np = np.array(image)
    R = image_np[:, :, 0]  # Красный канал
    G = image_np[:, :, 1]  # Зеленый канал
    B = image_np[:, :, 2]  # Синий канал
    return R, G, B
# Функция для создания изображения только с одним каналом (R, G или B)
def create_channel_image(image, channel):
    image_np = np.zeros_like(np.array(image))
    image_np[:, :, channel] = np.array(image)[:, :, channel]
    return Image.fromarray(image_np)
# Загрузка изображения
image = Image.open('GREG2.jpg')
plt.imshow(image)

# Извлечение каналов R, G, B
R_channel, G_channel, B_channel = extract_rgb_channels(image)

plt.figure(figsize=(12, 6))
# Построение гистограмм для каналов R, G, B
# Гистограмма для канала R
plt.subplot(2, 3, 1)
plt.hist(R_channel.ravel(), bins=256, range=(0, 256), color='red')
plt.title('Red Channel Histogram')

# Гистограмма для канала G
plt.subplot(2, 3, 2)
plt.hist(G_channel.ravel(), bins=256, range=(0, 256), color='green')
plt.title('Green Channel Histogram')

# Гистограмма для канала B
plt.subplot(2, 3, 3)
plt.hist(B_channel.ravel(), bins=256, range=(0, 256), color='blue')
plt.title('Blue Channel Histogram')

plt.subplot(2, 3, 4)
plt.imshow(create_channel_image(image, 0))
plt.title('Red Channel Image')

# Зеленый канал
plt.subplot(2, 3, 5)
plt.imshow(create_channel_image(image, 1))
plt.title('Green Channel Image')

# Синий канал
plt.subplot(2, 3, 6)
plt.imshow(create_channel_image(image, 2))
plt.title('Blue Channel Image')

# Отображение изображений
plt.tight_layout()
plt.show()

Image.fromarray(R_channel).save('R.jpg')
Image.fromarray(G_channel).save('G.jpg')
Image.fromarray(B_channel).save('B.jpg')