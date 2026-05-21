import unittest
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from DengLuCheSHI.WangJimiamagj import WangJiMiMaPage   # 假设你的 Loginpage 在这个模块
class wjmm_test(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        chrome_options = Options()
        # 设置一个非浏览器风格的 User-Agent，绕过 ngrok 警告页
        chrome_options.add_argument("--user-agent=MyTestRunner/1.0")

        cls.driver = webdriver.Chrome(options=chrome_options)
        cls.driver.maximize_window()
        cls.wj = WangJiMiMaPage(cls.driver)

    @classmethod
    def tearDownClass(cls):
        cls.driver.quit()

    def setUp(self):
        self.driver = self.__class__.driver
        self.driver.delete_all_cookies()
        # 请替换为实际的忘记密码页面地址
        self.wj.a1("https://aloft-rectify-salvaging.ngrok-free.dev/DengLu/WangJiMiMa")

    def test_wj1(self):
        """正确流程：所有字段正确，修改成功并跳转登录页"""
        self.wj.a2("10000010")
        self.wj.a3("new@test.com")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("密码修改成功", self.wj.a12())
        self.assertTrue(self.wj.a14(), "断言失败：未跳转到登录页")

    def test_wj2(self):
        """用户名为空"""
        self.wj.a2("")
        self.wj.a3("123@qq.com")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("账户不能为空", self.wj.a8())

    def test_wj3(self):
        """邮箱为空"""
        self.wj.a2("10000010")
        self.wj.a3("")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("邮箱不能为空", self.wj.a9())

    def test_wj4(self):
        """邮箱格式错误（缺少@）"""
        self.wj.a2("10000010")
        self.wj.a3("1452547270qq.com")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("邮箱格式不正确", self.wj.a9())

    def test_wj5(self):
        """邮箱格式错误（缺少.）"""
        self.wj.a2("10000010")
        self.wj.a3("1452547270@qqcom")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("邮箱格式不正确", self.wj.a9())

    def test_wj6(self):
        """新密码为空"""
        self.wj.a2("10000010")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("新密码不能为空", self.wj.a10())

    def test_wj7(self):
        """新密码长度小于6位"""
        self.wj.a2("10000010")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("12345")
        self.wj.a5("12345")
        self.wj.a6()
        self.assertIn("密码长度需在6-20位之间", self.wj.a10())

    def test_wj8(self):
        """新密码长度大于20位"""
        self.wj.a2("10000010")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("1" * 21)
        self.wj.a5("1" * 21)
        self.wj.a6()
        self.assertIn("密码长度需在6-20位之间", self.wj.a10())

    def test_wj9(self):
        """两次密码不一致"""
        self.wj.a2("10000010")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("12345678")
        self.wj.a5("87654321")
        self.wj.a6()
        self.assertIn("两次输入的密码不一致", self.wj.a11())

    def test_wj10(self):
        """确认密码为空"""
        self.wj.a2("10000010")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("12345678")
        self.wj.a5("")
        self.wj.a6()
        self.assertIn("两次输入的密码不一致", self.wj.a11())
    def test_wj11(self):
        """返回登录按钮功能"""
        self.wj.a7()
        self.assertTrue(self.wj.a14(), "断言失败：未跳转到登录页")
    def test_wj12(self):
        """账户邮箱和注册时不一样"""
        self.wj.a2("100000000")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("账户与邮箱不匹配，请检查后重试", self.wj.a13())
    def test_wj13(self):
        """改回来"""
        self.wj.a2("10000010")
        self.wj.a3("new@test.com")
        self.wj.a4("123456")
        self.wj.a5("123456")
        self.wj.a6()
        self.assertIn("密码修改成功", self.wj.a12())
        self.assertTrue(self.wj.a14(), "断言失败：未跳转到登录页")