from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait


class Loginpage:
    def __init__(self, driver):
        # 创建一个缓存方便调用
        self.driver = driver
        self.wait = WebDriverWait(driver, 10)
        # 账号
        self._zh = (By.ID, "TextBox1")
        # 密码
        self._mima = (By.ID, "TextBox2")
        # 登录
        self._dl = (By.CLASS_NAME, "denglu-anniu")
        # 点击贴子
        self._tjtz=(By.CSS_SELECTOR, "body > div > div.header > div:nth-child(1) > ul > li:nth-child(2) > a")
        #标题
        self._biaoti=(By.CSS_SELECTOR,"#tianjia-app > form > div:nth-child(1) > input")
        #图片
        self._tupian = (By.CSS_SELECTOR, "#tianjia-app > form > div:nth-child(2) > div > div")
        #贴子内容
        self._tzineir = (By.CSS_SELECTOR, "#tianjia-app > form > div:nth-child(3) > textarea")
        #发布贴子内容
        self._fabu = (By.CSS_SELECTOR, "#tianjia-app > form > input")
        #返回上一页
        self._fanhuishangyy=(By.CSS_SELECTOR,"#fanhuiBtn")

    def a1(self, url):
        # 封装一个登录的方法
        self.driver.get(url)
        self.wait.until(EC.presence_of_element_located(self._zh))

    def a2(self, gzy):
        # 输入账号
        a1 = self.wait.until(EC.element_to_be_clickable(self._zh))
        a1.clear()
        a1.send_keys(gzy)

    def a3(self, gzy):
        # 输入密码
        a1 = self.wait.until(EC.element_to_be_clickable(self._mima))
        a1.clear()
        a1.send_keys(gzy)

    def a5(self):
        # 点击登录
        a1 = self.wait.until(EC.element_to_be_clickable(self._dl))
        a1.click()

    def tianjiatzi(self):
        # 添加贴子
        a1 = self.wait.until(EC.element_to_be_clickable(self._tjtz))
        a1.click()
    def shurubiaoti(self, gzy):
        # 输入标题
        a1 = self.wait.until(EC.element_to_be_clickable(self._biaoti))
        a1.clear()
        a1.send_keys(gzy)
    def shuruzhangpian(self, gzy):
        # 输入张片
        a1 = self.wait.until(EC.element_to_be_clickable(self._tupian))
        a1.clear()
        a1.send_keys(gzy)
    def shuruneirong(self, gzy):
        # 输入贴子内容
        a1 = self.wait.until(EC.element_to_be_clickable(self._tzineir))
        a1.clear()
        a1.send_keys(gzy)
    def fabu(self):
        # 发布贴子内容
        a1 = self.wait.until(EC.element_to_be_clickable(self._fabu))
        a1.click()
    def fanhuishangyy(self):
        # 返回上一页
        a1 = self.wait.until(EC.element_to_be_clickable(self._fanhuishangyy))
        a1.click()
