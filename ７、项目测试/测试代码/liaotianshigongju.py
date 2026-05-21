from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
class Loginpage:
    def __init__(self,driver):
        #创建一个缓存方便调用
        self.driver = driver
        self.wait = WebDriverWait(driver,10)
        #账号
        self._zh=(By.ID,"TextBox1")
        #密码
        self._mima=(By.ID,"TextBox2")
        #登录
        self._dl=(By.CLASS_NAME,"denglu-anniu")
        #点击聊天室
        self._lts=(By.CSS_SELECTOR,"body > div > div.header > div:nth-child(1) > ul > li:nth-child(4) > a")
        #点击聊天室1
        self._lts1 = (By.CSS_SELECTOR, "#liaotian-app > div.ce-bian-lan > div.qun-liao-xiang.huoyue > span")
        #消息框
        self._xxk=(By.CSS_SELECTOR,"#liaotian-app > div.liaotian-qu > div.shuru-qu > input")
        #点击发送
        self._fs=(By.CSS_SELECTOR,"#liaotian-app > div.liaotian-qu > div.shuru-qu > button")
        #al助手
        self._alzs=(By.CSS_SELECTOR,"#liaotian-app > div.ce-bian-lan > div.qun-liao-xiang.huoyue > span")
        #意见反馈
        self._yjfk=(By.CSS_SELECTOR,"#liaotian-app > div.ce-bian-lan > div:nth-child(6) > span")
        #意见信息栏
        self._yjxxl=(By.CSS_SELECTOR,"#liaotian-app > div.liaotian-qu > div.fankui-form > textarea")
        #意见发送
        self._yjfs=(By.CSS_SELECTOR,"#liaotian-app > div.liaotian-qu > div.fankui-form > button")
    def a1(self,url):
        #封装一个登录的方法
        self.driver.get(url)
        self.wait.until(EC.presence_of_element_located(self._zh))
    def a2(self,gzy):
        # 输入账号
       a1=self.wait.until(EC.element_to_be_clickable(self._zh))
       a1.clear()
       a1.send_keys(gzy)
    def a3(self,gzy):
        #输入密码
        a1=self.wait.until(EC.element_to_be_clickable(self._mima))
        a1.clear()
        a1.send_keys(gzy)
    def a5(self):
        #点击登录
        a1=self.wait.until(EC.element_to_be_clickable(self._dl))
        a1.click()
    def a6(self):
        # 点击聊天室
        a1 = self.wait.until(EC.element_to_be_clickable(self._lts))
        a1.click()
    def a7(self,gzy):
        #输入消息
        a1=self.wait.until(EC.element_to_be_clickable(self._xxk))
        a1.clear()
        a1.send_keys(gzy)
    def a8(self):
        # 点击聊天室1
        a1 = self.wait.until(EC.element_to_be_clickable(self._lts1))
        a1.click()
    def a9(self):
        # 发送
        a1 = self.wait.until(EC.element_to_be_clickable(self._fs))
        a1.click()
    def a10(self):
        # 点击al
        a1 = self.wait.until(EC.element_to_be_clickable(self._alzs))
        a1.click()
    def a11(self):
        # 点击al
        a1 = self.wait.until(EC.element_to_be_clickable(self._yjfk))
        a1.click()
    def a12(self,gzy):
        #输入消息
        a1=self.wait.until(EC.element_to_be_clickable(self._yjxxl))
        a1.clear()
        a1.send_keys(gzy)
    def a13(self):
        # 点击al
        a1 = self.wait.until(EC.element_to_be_clickable(self._yjfs))
        a1.click()