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
        #获取账户输入为空的文本
        self._cwsr=(By.CSS_SELECTOR,"#denglu-app > form > div.denglu-zhuti > div:nth-child(1) > span:nth-child(3) > span")
        #获取密码输入为空的文本
        self._mimacw=(By.CSS_SELECTOR,"#denglu-app > form > div.denglu-zhuti > div:nth-child(2) > span:nth-child(3) > span")
        #获取失败文本的值
        self._cuoweitishi=(By.CLASS_NAME,"houduan-cuowu-tishi")
        #登录
        self._dl=(By.CLASS_NAME,"denglu-anniu")
        #记住密码
        self._jzmm=(By.ID,"CheckBox1")

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
        #获取失败文本的值
        a1=self.wait.until(EC.visibility_of_element_located(self._cuoweitishi))
        return a1.text
    def a8(self):
        # 获取账号为空文本的值
        a1 = self.wait.until(EC.visibility_of_element_located(self._cwsr))
        return a1.text
    def a7(self):
        return self.wait.until(EC.title_contains("Index"))
    def a9(self):
        # 获取账号为空文本的值
        a1 = self.wait.until(EC.visibility_of_element_located(self._mimacw))
        return a1.text
    def a10(self):
        #点击记住密码
        a1=self.wait.until(EC.element_to_be_clickable(self._jzmm))
        a1.click()