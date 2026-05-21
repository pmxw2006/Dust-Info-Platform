import unittest
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC


# ==================== 忘记密码页面对象 ====================
class WangJiMiMaPage:
    def __init__(self, driver):
        self.driver = driver
        self.wait = WebDriverWait(self.driver, 10)

        # 元素定位
        self._yhm = (By.ID, "YongHuMing")                     # 用户名
        self._yx = (By.ID, "YouXiang")                        # 邮箱
        self._xmm = (By.ID, "XinMiMa")                        # 新密码
        self._qrxmm = (By.ID, "QueRenXinMiMa")                # 确认密码
        self._xgmm_btn = (By.CSS_SELECTOR, ".AnNiuTianJia")   # 修改密码按钮
        self._fhdl_btn = (By.CSS_SELECTOR, ".AnNiuFanHui")    # 返回登录按钮

        # 错误提示
        self._yhm_error = (By.CSS_SELECTOR,
                           "#wangji-mima-app > form > div.denglu-zhuti > div:nth-child(2) > span:nth-child(3) > span")
        self._yx_error = (By.CSS_SELECTOR,
                          "#wangji-mima-app > form > div.denglu-zhuti > div:nth-child(3) > span:nth-child(3) > span")
        self._xmm_error = (By.CSS_SELECTOR,
                           "#wangji-mima-app > form > div.denglu-zhuti > div:nth-child(4) > span:nth-child(3) > span")
        self._qrxmm_error = (By.CSS_SELECTOR,
                             "#wangji-mima-app > form > div.denglu-zhuti > div:nth-child(5) > span:nth-child(3) > span")
        self._cgts = (By.CSS_SELECTOR,
                      "#wangji-mima-app > form > div.denglu-zhuti > div.chenggong-tishi")
        self._hderror = (By.CSS_SELECTOR,
                         "#wangji-mima-app > form > div.denglu-zhuti > div.cuowu-tishi-rongqi > div")

    def a1(self, url):
        """打开忘记密码页面"""
        self.driver.get(url)
        self.wait.until(EC.presence_of_element_located(self._yhm))

    def a2(self, zhi):
        """输入用户名"""
        elem = self.wait.until(EC.element_to_be_clickable(self._yhm))
        elem.clear()
        elem.send_keys(zhi)

    def a3(self, zhi):
        """输入邮箱"""
        elem = self.wait.until(EC.element_to_be_clickable(self._yx))
        elem.clear()
        elem.send_keys(zhi)

    def a4(self, zhi):
        """输入新密码"""
        elem = self.wait.until(EC.element_to_be_clickable(self._xmm))
        elem.clear()
        elem.send_keys(zhi)

    def a5(self, zhi):
        """输入确认密码"""
        elem = self.wait.until(EC.element_to_be_clickable(self._qrxmm))
        elem.clear()
        elem.send_keys(zhi)

    def a6(self):
        """点击修改密码按钮"""
        self.wait.until(EC.element_to_be_clickable(self._xgmm_btn)).click()

    def a7(self):
        """点击返回登录按钮"""
        self.wait.until(EC.element_to_be_clickable(self._fhdl_btn)).click()

    def a8(self):
        """获取用户名错误提示文本"""
        elem = self.wait.until(EC.visibility_of_element_located(self._yhm_error))
        return elem.text

    def a9(self):
        """获取邮箱错误提示文本"""
        elem = self.wait.until(EC.visibility_of_element_located(self._yx_error))
        return elem.text

    def a10(self):
        """获取新密码错误提示文本"""
        elem = self.wait.until(EC.visibility_of_element_located(self._xmm_error))
        return elem.text

    def a11(self):
        """获取确认密码错误提示文本"""
        elem = self.wait.until(EC.visibility_of_element_located(self._qrxmm_error))
        return elem.text

    def a12(self):
        """获取成功提示文本"""
        elem = self.wait.until(EC.visibility_of_element_located(self._cgts))
        return elem.text

    def a13(self):
        """获取后端错误提示文本"""
        elem = self.wait.until(EC.visibility_of_element_located(self._hderror))
        return elem.text

    def a14(self):
        """判断是否跳转回登录页"""
        return self.wait.until(EC.title_contains("微尘游戏咨询平台 - 登录"))


# ==================== 测试类 ====================
class wjmm_test(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        chrome_options = Options()
        # 绕过 ngrok 反滥用警告页面
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
        self.wj.a1("https://aloft-rectify-salvaging.ngrok-free.dev/DengLu/WangJiMiMa")

    # ---------- 测试用例 ----------
    def test_wj1(self):
        """正确流程：所有字段正确，修改成功并跳转登录页"""
        self.wj.a2("10000009")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("密码修改成功", self.wj.a12())
        self.assertTrue(self.wj.a14(), "断言失败：未跳转到登录页")

    def test_wj2(self):
        """用户名为空"""
        self.wj.a2("")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("账户不能为空", self.wj.a8())

    def test_wj3(self):
        """邮箱为空"""
        self.wj.a2("10000009")
        self.wj.a3("")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("邮箱不能为空", self.wj.a9())

    def test_wj4(self):
        """邮箱格式错误（缺少@）"""
        self.wj.a2("10000009")
        self.wj.a3("1452547270qq.com")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("邮箱格式不正确", self.wj.a9())

    def test_wj5(self):
        """邮箱格式错误（缺少.）"""
        self.wj.a2("10000009")
        self.wj.a3("1452547270@qqcom")
        self.wj.a4("12345678")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("邮箱格式不正确", self.wj.a9())

    def test_wj6(self):
        """新密码为空"""
        self.wj.a2("10000009")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("")
        self.wj.a5("12345678")
        self.wj.a6()
        self.assertIn("新密码不能为空", self.wj.a10())

    def test_wj7(self):
        """新密码长度小于6位"""
        self.wj.a2("10000009")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("12345")
        self.wj.a5("12345")
        self.wj.a6()
        self.assertIn("密码长度需在6-20位之间", self.wj.a10())

    def test_wj8(self):
        """新密码长度大于20位"""
        self.wj.a2("10000009")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("1" * 21)
        self.wj.a5("1" * 21)
        self.wj.a6()
        self.assertIn("密码长度需在6-20位之间", self.wj.a10())

    def test_wj9(self):
        """两次密码不一致"""
        self.wj.a2("10000009")
        self.wj.a3("1452547270@qq.com")
        self.wj.a4("12345678")
        self.wj.a5("87654321")
        self.wj.a6()
        self.assertIn("两次输入的密码不一致", self.wj.a11())

    def test_wj10(self):
        """确认密码为空"""
        self.wj.a2("10000009")
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


if __name__ == "__main__":
    unittest.main()