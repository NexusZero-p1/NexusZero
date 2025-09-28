#!/usr/bin/env python3
"""
Advanced Lyrics Display System with Particle Animations
Comprehensive Test Suite
"""

import unittest
import time
import sys
import random
import os
import math
import threading
from dataclasses import dataclass
from enum import Enum
from typing import List, Dict, Optional, Callable
from datetime import datetime
from unittest.mock import patch, MagicMock, call
import io
import contextlib

# ==================== CONFIGURATION ====================
class Config:
    """Configuration constants"""
    TYPING_DELAY = 0.01  # Faster for tests
    LINE_DELAY = 0.1
    EMPTY_LINE_DELAY = 0.2
    ANIMATION_FPS = 30
    SCREEN_WIDTH = 80
    SCREEN_HEIGHT = 24
    PARTICLE_COUNT = 20
    MAX_STARS = 30

# ==================== ANSI COLORS ====================
class Colors:
    """ANSI color codes with gradient support"""
    RESET = '\033[0m'
    BOLD = '\033[1m'

    # Pink palette
    PINK = '\033[95m'
    HOT_PINK = '\033[38;5;198m'
    DEEP_PINK = '\033[38;5;197m'
    NEON_PINK = '\033[38;5;200m'
    BLUSH_PINK = '\033[38;5;211m'

    # Purple palette
    PURPLE = '\033[38;5;93m'
    LAVENDER = '\033[38;5;183m'
    MAGENTA = '\033[38;5;201m'

    # Blue palette
    CYAN = '\033[96m'
    LIGHT_BLUE = '\033[38;5;45m'

    # Additional
    GOLD = '\033[38;5;220m'
    SILVER = '\033[38;5;7m'

    GRADIENTS = {
        'pink': [HOT_PINK, DEEP_PINK, NEON_PINK, BLUSH_PINK],
        'purple': [PURPLE, MAGENTA, LAVENDER],
        'rainbow': [HOT_PINK, GOLD, LIGHT_BLUE, MAGENTA, CYAN]
    }

    @staticmethod
    def get_gradient_color(text: str, palette: List[str]) -> str:
        """Apply gradient to text"""
        result = ""
        for i, char in enumerate(text):
            color = palette[i % len(palette)]
            result += f"{color}{char}"
        return result + Colors.RESET

# ==================== ANIMATION SYSTEM ====================
class ParticleType(Enum):
    STAR = "⭐"
    HEART = "❤️"
    MUSIC = "🎵"
    SPARKLE = "✨"
    DIAMOND = "💎"

@dataclass
class Particle:
    x: float
    y: float
    vx: float
    vy: float
    life: float
    max_life: float
    type: ParticleType
    color: str

class AnimationSystem:
    """Advanced particle animation system"""

    def __init__(self):
        self.particles: List[Particle] = []
        self.stars: List[Dict] = []
        self.running = False
        self.thread: Optional[threading.Thread] = None

    def start(self):
        """Start animation thread"""
        self.running = True
        self._create_stars()
        self.thread = threading.Thread(target=self._animation_loop, daemon=True)
        self.thread.start()

    def stop(self):
        """Stop animation thread"""
        self.running = False
        if self.thread:
            self.thread.join(timeout=0.5)

    def _create_stars(self):
        """Create background stars"""
        self.stars = []
        for _ in range(Config.MAX_STARS):
            self.stars.append({
                'x': random.randint(0, Config.SCREEN_WIDTH),
                'y': random.randint(0, Config.SCREEN_HEIGHT),
                'speed': random.uniform(0.1, 0.5),
                'brightness': random.uniform(0.3, 1.0)
            })

    def create_explosion(self, x: int, y: int, count: int = 20):
        """Create particle explosion"""
        for _ in range(count):
            angle = random.uniform(0, 2 * math.pi)
            speed = random.uniform(0.5, 2.0)
            particle_type = random.choice(list(ParticleType))

            self.particles.append(Particle(
                x=x, y=y,
                vx=math.cos(angle) * speed,
                vy=math.sin(angle) * speed,
                life=random.uniform(1.0, 3.0),
                max_life=random.uniform(1.0, 3.0),
                type=particle_type,
                color=random.choice(list(Colors.GRADIENTS['pink']))
            ))

    def _animation_loop(self):
        """Main animation loop"""
        try:
            while self.running:
                self._update_particles()
                self._update_stars()
                self._render()
                time.sleep(1.0 / Config.ANIMATION_FPS)
        except Exception as e:
            print(f"Animation error: {e}")

    def _update_particles(self):
        """Update particle positions and lifetimes"""
        new_particles = []
        for p in self.particles:
            p.x += p.vx
            p.y += p.vy
            p.life -= 0.05
            p.vy += 0.1  # Gravity

            if p.life > 0 and 0 <= p.x < Config.SCREEN_WIDTH and 0 <= p.y < Config.SCREEN_HEIGHT:
                new_particles.append(p)

        self.particles = new_particles

    def _update_stars(self):
        """Update star positions"""
        for star in self.stars:
            star['x'] -= star['speed']
            if star['x'] < 0:
                star['x'] = Config.SCREEN_WIDTH
                star['y'] = random.randint(0, Config.SCREEN_HEIGHT)

    def _render(self):
        """Render all animations"""
        buffer = [[' ' for _ in range(Config.SCREEN_WIDTH)] for _ in range(Config.SCREEN_HEIGHT)]

        # Render stars
        for star in self.stars:
            x, y = int(star['x']), int(star['y'])
            if 0 <= x < Config.SCREEN_WIDTH and 0 <= y < Config.SCREEN_HEIGHT:
                brightness = int(star['brightness'] * 5)
                buffer[y][x] = f"{Colors.SILVER}•{Colors.RESET}" if brightness > 2 else '.'

        # Render particles
        for p in self.particles:
            x, y = int(p.x), int(p.y)
            if 0 <= x < Config.SCREEN_WIDTH and 0 <= y < Config.SCREEN_HEIGHT:
                alpha = p.life / p.max_life
                buffer[y][x] = f"{p.color}{p.type.value}{Colors.RESET}"

        # Clear and draw
        sys.stdout.write('\033[H')  # Move cursor to top
        for row in buffer:
            sys.stdout.write(''.join(row) + '\n')
        sys.stdout.flush()

# ==================== TEXT RENDERER ====================
class TextRenderer:
    """Advanced text rendering with effects"""

    @staticmethod
    def clear_screen():
        """Clear terminal screen"""
        os.system('cls' if os.name == 'nt' else 'clear')
        sys.stdout.write('\033[?25l')  # Hide cursor
        sys.stdout.flush()

    @staticmethod
    def show_cursor():
        """Show cursor"""
        sys.stdout.write('\033[?25h')
        sys.stdout.flush()

    @staticmethod
    def type_with_effect(text: str, delay: float = Config.TYPING_DELAY,
                        effect: Optional[Callable] = None, **kwargs):
        """Type text with optional effect"""
        if effect:
            effect(text, delay, **kwargs)
        else:
            for char in text:
                sys.stdout.write(char)
                sys.stdout.flush()
                time.sleep(delay)
            print()

    @staticmethod
    def rainbow_effect(text: str, delay: float, **kwargs):
        """Rainbow typing effect"""
        palette = kwargs.get('palette', Colors.GRADIENTS['rainbow'])
        for i, char in enumerate(text):
            color = palette[i % len(palette)]
            sys.stdout.write(f"{color}{char}{Colors.RESET}")
            sys.stdout.flush()
            time.sleep(delay)
        print()

    @staticmethod
    def glow_effect(text: str, delay: float, **kwargs):
        """Glowing text effect"""
        base_color = kwargs.get('color', Colors.HOT_PINK)
        for i, char in enumerate(text):
            intensity = 0.5 + 0.5 * math.sin(i * 0.5 + time.time() * 5)
            sys.stdout.write(f"{base_color}{Colors.BOLD}{char}{Colors.RESET}")
            sys.stdout.flush()
            time.sleep(delay)
        print()

# ==================== LYRICS MANAGER ====================
class LyricsManager:
    """Manage lyrics and timing"""

    def __init__(self):
        self.lyrics = [
            ("Mhm, they're rotting my brain, love", {}),
            ("These hoes are the same", {'delay': 0.5}),
            ("", {'pause': 1.0}),
            ("[Verse 1]", {'effect': TextRenderer.rainbow_effect, 'palette': Colors.GRADIENTS['purple']}),
            ("I admit it, another ho got me finished", {'color': Colors.HOT_PINK}),
            ("Broke my heart, oh, no, you didn't", {'color': Colors.DEEP_PINK}),
            ("Fuck sippin', I'ma down a whole bottle", {'effect': TextRenderer.glow_effect}),
            ("Hard liquor, hard truth, can't swallow", {'color': Colors.NEON_PINK}),
            ("Need a bartender, put me out my sorrow", {'delay': 0.4}),
            ("Wake up the next day in the Monte Carlo", {'color': Colors.MAGENTA}),
            ("With a new woman, tell me she from Colorado", {'effect': TextRenderer.rainbow_effect}),
            ("And she love women, she'll be gone by tomorrow", {'color': Colors.LAVENDER}),
            ("Who am I kiddin'?", {'delay': 0.6}),
            ("All this jealousy and agony that I sit in", {'color': Colors.BLUSH_PINK}),
            ("I'm a jealous boy, really feel like John Lennon", {'effect': TextRenderer.glow_effect}),
            ("I just want real love, guess it's been a minute", {'color': Colors.CYAN}),
            ("Pissed off from the way that I don't fit in (I don't fit in)", {'delay': 0.5}),
            ("Tell me, what's the secret to love? I don't get it", {'color': Colors.GOLD}),
            ("Feel like I be runnin' a race I'm not winnin'", {'effect': TextRenderer.rainbow_effect}),
            ("Ran into the devil today and she grinnin'", {'color': Colors.HOT_PINK, 'delay': 0.4})
        ]

    def get_lyrics(self):
        """Get lyrics with metadata"""
        return self.lyrics

# ==================== MAIN APPLICATION ====================
class MusicVisualizerApp:
    """Main application class"""

    def __init__(self):
        self.animation_system = AnimationSystem()
        self.text_renderer = TextRenderer()
        self.lyrics_manager = LyricsManager()
        self.is_running = False

    def setup(self):
        """Setup application"""
        self.text_renderer.clear_screen()
        self._print_header()
        self.animation_system.start()

    def cleanup(self):
        """Cleanup resources"""
        self.animation_system.stop()
        self.text_renderer.show_cursor()
        print(f"\n{Colors.GOLD}🎶 Thank you for watching! 🎶{Colors.RESET}")

    def _print_header(self):
        """Print application header"""
        title = "✨ ULTIMATE MUSIC VISUALIZER ✨"
        now = datetime.now().strftime("%Y-%m-%d %H:%M:%S")

        print(f"{Colors.BOLD}{Colors.get_gradient_color(title, Colors.GRADIENTS['rainbow'])}")
        print("=" * 60)
        print(f"{Colors.SILVER}Now Playing: Heartbreak Symphony")
        print(f"Time: {now}{Colors.RESET}")
        print("=" * 60)
        print()

    def run(self):
        """Main application loop"""
        try:
            self.is_running = True
            self.setup()

            lyrics = self.lyrics_manager.get_lyrics()

            for i, (line, metadata) in enumerate(lyrics):
                if not self.is_running:
                    break

                if not line.strip():  # Empty line
                    time.sleep(metadata.get('pause', Config.EMPTY_LINE_DELAY))
                    continue

                # Create explosion every few lines
                if i % 3 == 0:
                    self.animation_system.create_explosion(
                        random.randint(10, 70),
                        random.randint(5, 20),
                        random.randint(15, 30)
                    )

                # Render text with effects
                delay = metadata.get('delay', Config.TYPING_DELAY)
                effect = metadata.get('effect', None)
                color = metadata.get('color', Colors.HOT_PINK)

                if effect:
                    effect(line, delay, **metadata)
                else:
                    sys.stdout.write(color)
                    self.text_renderer.type_with_effect(line, delay)
                    sys.stdout.write(Colors.RESET)

                time.sleep(metadata.get('line_delay', Config.LINE_DELAY))

            # Final explosion
            self.animation_system.create_explosion(40, 12, 100)
            time.sleep(3)

        except KeyboardInterrupt:
            print(f"\n{Colors.PINK}⏹️  Stopping visualizer...{Colors.RESET}")
        finally:
            self.is_running = False
            self.cleanup()

# ==================== COMPREHENSIVE TEST SUITE ====================
class TestAnimationSystem(unittest.TestCase):
    """Test AnimationSystem functionality"""

    def setUp(self):
        self.anim_system = AnimationSystem()

    def test_initialization(self):
        """Test AnimationSystem initialization"""
        self.assertEqual(len(self.anim_system.particles), 0)
        self.assertEqual(len(self.anim_system.stars), 0)
        self.assertFalse(self.anim_system.running)
        self.assertIsNone(self.anim_system.thread)

    def test_create_stars(self):
        """Test star creation"""
        self.anim_system._create_stars()
        self.assertEqual(len(self.anim_system.stars), Config.MAX_STARS)

        for star in self.anim_system.stars:
            self.assertIn('x', star)
            self.assertIn('y', star)
            self.assertIn('speed', star)
            self.assertIn('brightness', star)

    def test_create_explosion(self):
        """Test particle explosion creation"""
        initial_count = len(self.anim_system.particles)
        self.anim_system.create_explosion(10, 10, 5)
        self.assertEqual(len(self.anim_system.particles), initial_count + 5)

    def test_particle_update(self):
        """Test particle physics update"""
        particle = Particle(10.0, 10.0, 1.0, -1.0, 2.0, 2.0, ParticleType.HEART, Colors.HOT_PINK)
        self.anim_system.particles.append(particle)

        self.anim_system._update_particles()

        self.assertAlmostEqual(particle.x, 11.0)
        self.assertAlmostEqual(particle.y, 9.1)  # 10 + (-1) + 0.1 gravity
        self.assertAlmostEqual(particle.life, 1.95)

    def test_star_update(self):
        """Test star movement update"""
        self.anim_system._create_stars()
        star = self.anim_system.stars[0]
        original_x = star['x']

        self.anim_system._update_stars()

        self.assertAlmostEqual(star['x'], original_x - star['speed'])

class TestTextRenderer(unittest.TestCase):
    """Test TextRenderer functionality"""

    def test_clear_screen(self):
        """Test screen clearing (mocked)"""
        with patch('os.system') as mock_system, \
             patch('sys.stdout.write') as mock_write:
            TextRenderer.clear_screen()
            mock_system.assert_called_once()
            mock_write.assert_called_with('\033[?25l')

    def test_show_cursor(self):
        """Test cursor showing"""
        with patch('sys.stdout.write') as mock_write:
            TextRenderer.show_cursor()
            mock_write.assert_called_with('\033[?25h')

    def test_rainbow_effect(self):
        """Test rainbow text effect"""
        test_text = "test"
        with patch('sys.stdout.write') as mock_write, \
             patch('time.sleep'):
            TextRenderer.rainbow_effect(test_text, 0.01)

            # Should write each character with color
            self.assertGreaterEqual(mock_write.call_count, len(test_text))

    def test_glow_effect(self):
        """Test glow text effect"""
        test_text = "glow"
        with patch('sys.stdout.write') as mock_write, \
             patch('time.sleep'):
            TextRenderer.glow_effect(test_text, 0.01)
            self.assertGreaterEqual(mock_write.call_count, len(test_text))

class TestLyricsManager(unittest.TestCase):
    """Test LyricsManager functionality"""

    def setUp(self):
        self.manager = LyricsManager()

    def test_lyrics_loading(self):
        """Test lyrics are loaded correctly"""
        lyrics = self.manager.get_lyrics()
        self.assertIsInstance(lyrics, list)
        self.assertGreater(len(lyrics), 0)

        for line, metadata in lyrics:
            self.assertIsInstance(line, str)
            self.assertIsInstance(metadata, dict)

class TestColors(unittest.TestCase):
    """Test Colors functionality"""

    def test_gradient_creation(self):
        """Test gradient color generation"""
        test_text = "hello"
        result = Colors.get_gradient_color(test_text, Colors.GRADIENTS['rainbow'])

        self.assertIsInstance(result, str)
        self.assertIn(Colors.RESET, result)
        self.assertNotEqual(result, test_text)  # Should be colored

class TestMusicVisualizerApp(unittest.TestCase):
    """Test main application"""

    def setUp(self):
        self.app = MusicVisualizerApp()

    def test_app_initialization(self):
        """Test app initialization"""
        self.assertIsInstance(self.app.animation_system, AnimationSystem)
        self.assertIsInstance(self.app.text_renderer, TextRenderer)
        self.assertIsInstance(self.app.lyrics_manager, LyricsManager)
        self.assertFalse(self.app.is_running)

    def test_header_printing(self):
        """Test header printing (mocked)"""
        with patch('builtins.print') as mock_print, \
             patch('datetime.datetime') as mock_datetime:
            mock_datetime.now.return_value.strftime.return_value = "2023-01-01 12:00:00"

            self.app._print_header()

            self.assertGreater(mock_print.call_count, 0)

# ==================== PERFORMANCE TESTS ====================
class TestPerformance(unittest.TestCase):
    """Performance and stress tests"""

    def test_animation_performance(self):
        """Test animation system performance"""
        anim_system = AnimationSystem()
        anim_system._create_stars()

        # Add many particles
        for _ in range(100):
            anim_system.create_explosion(40, 12, 10)

        start_time = time.time()
        anim_system._update_particles()
        anim_system._update_stars()
        end_time = time.time()

        # Should complete quickly
        self.assertLess(end_time - start_time, 0.1)

    def test_memory_usage(self):
        """Test memory efficiency"""
        import tracemalloc

        tracemalloc.start()

        # Create multiple systems
        systems = [AnimationSystem() for _ in range(10)]
        for system in systems:
            system._create_stars()
            system.create_explosion(40, 12, 20)

        current, peak = tracemalloc.get_traced_memory()
        tracemalloc.stop()

        # Should use reasonable memory
        self.assertLess(peak, 10 * 1024 * 1024)  # < 10MB

# ==================== INTEGRATION TESTS ====================
class TestIntegration(unittest.TestCase):
    """Integration tests"""

    def test_full_workflow(self):
        """Test complete workflow without actual display"""
        app = MusicVisualizerApp()

        with patch.object(app.animation_system, 'start'), \
             patch.object(app.animation_system, 'stop'), \
             patch.object(app.animation_system, 'create_explosion'), \
             patch.object(app.text_renderer, 'clear_screen'), \
             patch.object(app.text_renderer, 'show_cursor'), \
             patch.object(app.text_renderer, 'type_with_effect'), \
             patch('builtins.print'), \
             patch('time.sleep'):

            app.setup()
            app.cleanup()

            # Verify interactions
            app.animation_system.start.assert_called_once()
            app.animation_system.stop.assert_called_once()

# ==================== TEST RUNNER ====================
def run_tests():
    """Run all tests with beautiful output"""
    print(f"{Colors.BOLD}{Colors.HOT_PINK}🎵 Starting Comprehensive Test Suite...{Colors.RESET}")
    print(f"{Colors.CYAN}═" * 60)

    loader = unittest.TestLoader()
    suite = unittest.TestSuite()

    # Add all test classes
    test_classes = [
        TestAnimationSystem,
        TestTextRenderer,
        TestLyricsManager,
        TestColors,
        TestMusicVisualizerApp,
        TestPerformance,
        TestIntegration
    ]

    for test_class in test_classes:
        suite.addTests(loader.loadTestsFromTestCase(test_class))

    # Run tests with colored output
    runner = unittest.TextTestRunner(verbosity=2, stream=sys.stdout)
    result = runner.run(suite)

    print(f"{Colors.CYAN}═" * 60)
    if result.wasSuccessful():
        print(f"{Colors.GOLD}✅ All tests passed! {len(result.failures)} failures, {len(result.errors)} errors{Colors.RESET}")
    else:
        print(f"{Colors.DEEP_PINK}❌ Tests failed: {len(result.failures)} failures, {len(result.errors)} errors{Colors.RESET}")

    return result.wasSuccessful()

# ==================== DEMO MODE ====================
def demo_mode():
    """Run a demo of the system"""
    print(f"{Colors.BOLD}{Colors.get_gradient_color('🎭 DEMO MODE 🎭', Colors.GRADIENTS['rainbow'])}")
    print(f"{Colors.SILVER}Showing quick demonstration...{Colors.RESET}")

    # Quick test of key components
    anim_system = AnimationSystem()
    anim_system._create_stars()
    anim_system.create_explosion(40, 12, 5)

    print(f"{Colors.GREEN}✅ Animation system: OK")

    # Test text effects
    with patch('time.sleep'):
        TextRenderer.rainbow_effect("Rainbow test", 0.001)
        TextRenderer.glow_effect("Glow test", 0.001)

    print(f"{Colors.GREEN}✅ Text effects: OK")

    # Test lyrics
    manager = LyricsManager()
    lyrics = manager.get_lyrics()
    print(f"{Colors.GREEN}✅ Lyrics loaded: {len(lyrics)} lines")

    print(f"{Colors.GOLD}🎉 Demo completed successfully!{Colors.RESET}")

# ==================== MAIN EXECUTION ====================
if __name__ == "__main__":
    # Parse command line arguments
    import argparse
    parser = argparse.ArgumentParser(description="Music Visualizer with Tests")
    parser.add_argument('--test', action='store_true', help='Run tests')
    parser.add_argument('--demo', action='store_true', help='Run demo')
    parser.add_argument('--run', action='store_true', help='Run main application')

    args = parser.parse_args()

    if args.test:
        success = run_tests()
        sys.exit(0 if success else 1)
    elif args.demo:
        demo_mode()
    elif args.run:
        app = MusicVisualizerApp()
        app.run()
    else:
        print(f"{Colors.BOLD}Usage:{Colors.RESET}")
        print("  python script.py --test   # Run comprehensive tests")
        print("  python script.py --demo   # Run quick demo")
        print("  python script.py --run    # Run main application")